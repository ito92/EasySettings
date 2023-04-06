using System;

namespace DaBois.Settings
{
    public class EasySettingsAttribute : Attribute
    {
        public enum scope { User, Project }

        public EasySettingsAttribute(string displayPath, string filename, string title, scope settingsScope, string[] tags = null)
        {
            this.filename = filename;
            this.displayPath = displayPath;
            this.title = title;
            this.tags = tags;
#if UNITY_EDITOR
            switch(settingsScope)
            {
                case scope.User:
                this.settingsScope = UnityEditor.SettingsScope.User;
                break;
                case scope.Project:
                this.settingsScope = UnityEditor.SettingsScope.Project;
                break;
            }            
#endif
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