using System.IO;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DaBois.Settings
{
    public abstract class EasySettingsProvider<T> : ScriptableObject where T : EasySettingsProvider<T>
    {
        public static T Instance => _instance != null ? _instance : Initialize();
        protected static T _instance;

        private static EasySettingsAttribute Attribute => _attribute != null ? _attribute : _attribute = typeof(T).GetCustomAttribute<EasySettingsAttribute>(true);
        static EasySettingsAttribute _attribute;

#if !UNITY_EDITOR
        private void OnEnable()
        {
            RuntimeInit();
        }
#endif

        public abstract void RuntimeInit();

        protected static T Initialize()
        {
            if (_instance != null)
            {
                return _instance;
            }

            // Verify there was a [Settings] attribute.
            if (Attribute == null)
            {
                Debug.LogError("[Settings] attribute missing for type: " + typeof(T).Name);
                return null;
            }

            // Attempt to load the settings asset.
            var filename = Attribute.filename ?? typeof(T).Name;
            var path = GetSettingsPath() + filename + ".asset";

#if UNITY_EDITOR
            _instance = AssetDatabase.LoadAssetAtPath<T>(path);
            if (_instance != null)
            {
                return _instance;
            }

            //Move asset to the correct path if already exists
            var instances = Resources.FindObjectsOfTypeAll<T>();
            if (instances.Length > 0)
            {
                var oldPath = AssetDatabase.GetAssetPath(instances[0]);
                var result = AssetDatabase.MoveAsset(oldPath, path);

                if(oldPath == path)
                {
                    Debug.Log(instances[0] + " is in the correct path. Skipping moving");
                    _instance = instances[0];
                    return _instance;
                }
                else if (string.IsNullOrEmpty(result))
                {
                    _instance = instances[0];
                    return _instance;
                }
                else
                {
                    Debug.LogWarning($"Failed to move previous settings asset " + $"'{oldPath}' to '{path}'. " + $"A new settings asset will be created.", _instance);
                }
            }
            if (_instance != null)
            {
                return _instance;
            }
            _instance = CreateInstance<T>();
#endif

#if UNITY_EDITOR
            Directory.CreateDirectory(Path.Combine(
                Directory.GetCurrentDirectory(),
                Path.GetDirectoryName(path)));

            AssetDatabase.CreateAsset(_instance, path);
            AssetDatabase.Refresh();
#endif
            return _instance;
        }

        static string GetSettingsPath()
        {
            return "Assets/Settings/";
        }

#if UNITY_EDITOR
        private static Editor _editor;

        public virtual SettingsProvider GenerateProvider()
        {
            var provider = new SettingsProvider(Attribute.displayPath, Attribute.settingsScope)
            {
                label = Attribute.title,
                guiHandler = (searchContext) =>
                {
                    var settings = Instance;

                    if (!_editor)
                    {
                        _editor = Editor.CreateEditor(Instance);
                    }
                    _editor.OnInspectorGUI();
                },

                keywords = Attribute.tags != null ? Attribute.tags : new string[0]
            };

            return provider;
        }
#endif
    }
}