using System;

namespace DaBois.Settings
{
    public class EasySettingsAttribute : Attribute
    {
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
    }
}