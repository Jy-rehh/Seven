using System.Threading.Tasks;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Interfaces
{
    public interface ISettingsRepository
    {
        Task<Settings> GetSettingsByUsernameAsync(string username);  // Async method to get settings by username
        Task UpdateSettings(string username, SettingsViewModel model);  // Method to update settings
        Task ResetSettingsToDefault(string username);  // Method to reset settings to default
    }
}




