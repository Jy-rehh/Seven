using System;

namespace ASI.Basecode.Data.Models
{
    public partial class Settings
    {
        public int SettingsId { get; set; }
        public string Username { get; set; }
        public string ThemePreference { get; set; }
        public bool NotificationsEnabled { get; set; }
        public string PreferredLanguage { get; set; }
        public DateTime LastUpdated { get; set; }
        public string CustomSettingsJson { get; set; }
    }
}

