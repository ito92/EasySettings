using System;

namespace DaBois.Settings
{
    public class EasySettingsAttribute : Attribute
    {
        public EasySettingsAttribute(string displayPath, string filename, string title,
    #if UNITY_EDITOR
        UnityEditor.SettingsScope settingsScope,
    #endif
         string[] tags = null)
        {
            this.filename = filename;
            this.displayPath = displayPath;
            this.title = title;
            this.tags = tags;
#if UNITY_EDITOR
            this.settingsScope = settingsScope;
#endif
        }

        public EasySettingsAttribute(string displayPath, string filename, string title, string[] tags = null)
        {
            this.filename = filename;
            this.displayPath = displayPath;
            this.title = title;
            this.tags = tags;
        }

        public readonly string displayPath;
        public readonly string filename;
        public readonly string title;
        public readonly string[] tags;
        #if UNITY_EDITOR
        public readonly UnityEditor.SettingsScope settingsScope = UnityEditor.SettingsScope.Project;
        #endif
    }
}