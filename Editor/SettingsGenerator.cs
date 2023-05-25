using UnityEngine;
using UnityEditor;
using System.IO;

namespace DaBois.Settings.Editor
{
    public class SettingsGenerator : EditorWindow
    {
        private string _className = "MySettings";
        private string _path = "My Settings/Sub Settings";
        private string _fileName = "NewMySettings";
        private string _title = "My settings title";
        private string[] _tags = new string[0];
        private EasySettingsAttribute.scope _scope = EasySettingsAttribute.scope.Project;

        [MenuItem("DaBois/Easy Settings/Generate new settings")]
        private static void Init()
        {
            SettingsGenerator window = (SettingsGenerator)EditorWindow.GetWindow(typeof(SettingsGenerator));
            window.titleContent = new GUIContent("Settings Generator");
            window.ShowUtility();
        }

        private void OnGUI()
        {
            _className = EditorGUILayout.TextField(new GUIContent("Class Name", "Don't use special symbols or space!"), _className);

            _className = _className.Replace(" ", string.Empty);

            _path = EditorGUILayout.TextField("Settings Path", _path);
            _fileName = EditorGUILayout.TextField("Asset Name", _fileName);
            _title = EditorGUILayout.TextField("Title", _title);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Scope");
            _scope = (EasySettingsAttribute.scope)EditorGUILayout.EnumPopup(_scope);
            EditorGUILayout.EndHorizontal();

            int newSize = _tags.Length;
            newSize = EditorGUILayout.DelayedIntField("Tags", newSize);

            if (newSize < 0)
            {
                newSize = 0;
            }

            if (newSize < _tags.Length)
            {
                if (newSize > 0)
                {
                    string[] oldValues = new string[newSize];
                    for (int i = 0; i < oldValues.Length; i++)
                    {
                        oldValues[i] = _tags[i];
                    }
                    _tags = oldValues;
                }
                else
                {
                    _tags = new string[0];
                }
            }
            else if (newSize > _tags.Length)
            {
                if (newSize > 1)
                {
                    string[] oldValues = new string[newSize];
                    for (int i = 0; i < _tags.Length; i++)
                    {
                        oldValues[i] = _tags[i];
                    }
                    oldValues[oldValues.Length - 1] = oldValues[oldValues.Length - 2];
                    _tags = oldValues;
                }
                else
                {
                    _tags = new string[1];
                }
            }

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical();

            for (int i = 0; i < _tags.Length; i++)
            {
                _tags[i] = EditorGUILayout.TextField("Tag " + i, _tags[i]);
            }

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;

            if (GUILayout.Button("Generate"))
            {
                GenerateSettings();
            }
        }

        private void GenerateSettings()
        {
            string path = Application.dataPath + "/Scripts/Settings/" + _className + ".cs";

            FileInfo fi = new FileInfo(path);
            if (!fi.Directory.Exists)
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            using (StreamWriter sf =
                     new StreamWriter(path))
            {
                sf.WriteLine("using DaBois.Settings;");
                sf.WriteLine("using UnityEngine;");
                sf.WriteLine("#if UNITY_EDITOR");
                sf.WriteLine("using UnityEditor;");
                sf.WriteLine("#endif");
                sf.WriteLine("");

                //Tags
                string tagsString = "";
                for (int i = 0; i < _tags.Length; i++)
                {
                    tagsString += "\"" + _tags[i] + "\"" + (i + 1 < _tags.Length ? ", " : "");
                }

                sf.WriteLine("[EasySettings(\"" + _path + "\", \"" + _fileName + "\", \"" + _title + "\", " + GetFullEnumName(_scope) + ", new string[] { " + tagsString + " })]");
                sf.WriteLine("public class " + _className + " : EasySettingsProvider<" + _className + ">");
                sf.WriteLine("{");
                sf.WriteLine("    public override void RuntimeInit()");
                sf.WriteLine("    {");
                sf.WriteLine("        _instance = this;");
                sf.WriteLine("    }");
                sf.WriteLine("}");
                sf.WriteLine("");
                sf.WriteLine("#if UNITY_EDITOR");
                sf.WriteLine("static class " + _className + "Register");
                sf.WriteLine("{");
                sf.WriteLine("    [SettingsProvider]");
                sf.WriteLine("    public static SettingsProvider CreateSettingsProvider()");
                sf.WriteLine("    {");
                sf.WriteLine("        return " + _className + ".Instance.GenerateProvider();");
                sf.WriteLine("    }");
                sf.WriteLine("}");
                sf.WriteLine("#endif");
            }

            AssetDatabase.Refresh();

            Close();
        }

        private string GetFullEnumName(System.Enum en)
        {
            return string.Format("{0}.{1}", en.GetType().Name, en.ToString());
        }
    }
}