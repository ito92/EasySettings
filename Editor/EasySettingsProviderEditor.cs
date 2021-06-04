using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DaBois.Settings.Editor
{
    [CustomEditor(typeof(EasySettingsProvider<>), true)]
    public class EasySettingsProviderEditor : UnityEditor.Editor
    {
        private List<Object> _preloadedAssets;
        static readonly string[] _excludedFields = { "m_Script" };

        private void Awake()
        {
            _preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
        }

        public override void OnInspectorGUI() => DrawDefaultInspector();

        protected new bool DrawDefaultInspector()
        {
            if (serializedObject.targetObject == null) return false;

            EditorGUI.BeginChangeCheck();
            serializedObject.UpdateIfRequiredOrScript();

            DrawPropertiesExcluding(serializedObject, _excludedFields);

            serializedObject.ApplyModifiedProperties();

            if (!_preloadedAssets.Contains(target))
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.HelpBox("Asset is not added to the list of preloaded Assets", MessageType.Warning);
                if (GUILayout.Button("Set as preloaded Asset"))
                {
                    _preloadedAssets.Add(target);
                    PlayerSettings.SetPreloadedAssets(_preloadedAssets.ToArray());
                }
                EditorGUILayout.EndVertical();
            }

            serializedObject.ApplyModifiedProperties();

            GUILayout.FlexibleSpace();
            GUI.enabled = false;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField("This Asset", serializedObject.targetObject, typeof(EasySettingsProvider<>), false);
            GUI.enabled = true;

            if (GUILayout.Button("ping"))
            {
                EditorGUIUtility.PingObject(serializedObject.targetObject);
            }

            EditorGUILayout.EndHorizontal();

            return EditorGUI.EndChangeCheck();
        }

    }
}