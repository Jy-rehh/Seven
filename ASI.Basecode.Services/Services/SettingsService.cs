using ASI.Basecode.Data.Interfaces;  // Import the ISettingsRepository interface
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Models;  // Import the SettingsViewModel
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;  // Dependency injection of the repository

        // Constructor to inject ISettingsRepository
        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        // Fetch user settings by username
        public async Task<SettingsViewModel> GetUserSettings(string username)
        {
            // Fetch settings from the repository asynchronously
            var settings = await _settingsRepository.GetSettingsByUsernameAsync(username);

            // If settings are found, map to SettingsViewModel. Otherwise, return a new instance.
            if (settings != null)
            {
                // Manually map Settings to SettingsViewModel
                var model = new SettingsViewModel
                {
                    Username = settings.Username,
                    Field1 = settings.Field1,  // Replace with actual fields from Settings
                    Field2 = settings.Field2   // Replace with actual fields from Settings
                };
                return model;
            }

            return new SettingsViewModel();  // Return an empty ViewModel if no settings found
        }

        // Update user settings
        public async Task UpdateUserSettings(string username, SettingsViewModel model)
        {
            // Map SettingsViewModel to Settings before updating
            var settings = new Settings
            {
                Username = username,
                Field1 = model.Field1,  // Replace with actual fields from SettingsViewModel
                Field2 = model.Field2   // Replace with actual fields from SettingsViewModel
            };

            // Update the settings in the repository asynchronously
            await _settingsRepository.UpdateSettings(username, settings);
        }

        // Reset user settings to default
        public async Task ResetUserSettings(string username)
        {
            // Reset the settings in the repository asynchronously
            await _settingsRepository.ResetSettingsToDefault(username);
        }
    }
}
