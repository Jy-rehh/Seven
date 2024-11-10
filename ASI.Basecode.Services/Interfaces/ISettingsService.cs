using ASI.Basecode.WebApp.Models;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface ISettingsService
    {
        Task<SettingsViewModel> GetUserSettings(string username);
        Task UpdateUserSettings(string username, SettingsViewModel model);
        Task ResetUserSettings(string username);
    }
}
