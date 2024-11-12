using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;

namespace ASI.Basecode.Services.Services
{
    public class SettingsService : ISettingsService
    {
        private Settings _settings = new Settings
        {
            Username = "DefaultUser",  // Example default value
            Field1 = "Field1Value",    // Example default value
            Field2 = "Field2Value"     // Example default value
        };

        // Implement GetSettings from ISettingsService
        public Settings GetSettings()
        {
            return _settings;
        }

        // Implement SaveSettings from ISettingsService
        public void SaveSettings(string username, string field1, string field2)
        {
            _settings.Username = username;
            _settings.Field1 = field1;
            _settings.Field2 = field2;
        }

        // Implement GetUserSettings from ISettingsService
        public Settings GetUserSettings(string username)
        {
            // Example logic: return user settings based on the username
            // In a real application, you would likely fetch this data from a database
            return new Settings
            {
                Username = username,
                Field1 = "UserField1",
                Field2 = "UserField2"
            };
        }

        // Implement UpdateUserSettings from ISettingsService
        public void UpdateUserSettings(string username, SettingsViewModel settings)
        {
            // Example logic: update user settings
            _settings.Username = username;
            _settings.Field1 = settings.Field1;
            _settings.Field2 = settings.Field2;
        }

        // Implement ResetUserSettings from ISettingsService
        public void ResetUserSettings(string username)
        {
            // Example logic: reset the user's settings to default values
            _settings.Username = "DefaultUser";
            _settings.Field1 = "DefaultField1";
            _settings.Field2 = "DefaultField2";
        }
    }
}


