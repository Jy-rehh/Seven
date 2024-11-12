using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Services.Interfaces
{
    public interface ISettingsService
    {
        // Existing methods
        Settings GetSettings();
        void SaveSettings(string username, string field1, string field2);

        // New methods
        Settings GetUserSettings(string username);
        void UpdateUserSettings(string username, SettingsViewModel settings);
        void ResetUserSettings(string username);
    }
}

