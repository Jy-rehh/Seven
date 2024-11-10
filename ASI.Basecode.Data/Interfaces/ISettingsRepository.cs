using ASI.Basecode.Data.Models;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface ISettingsRepository
    {
        Task<Settings> GetSettingsByUsernameAsync(string username);  // Async method to get settings by username
        Task UpdateSettings(string username, Settings settings);  // Method to update settings
        Task ResetSettingsToDefault(string username);  // Method to reset settings to default
    }
}





