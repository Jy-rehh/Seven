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
        void UpdateSettings(ExpenseViewModel expenseModel, UserViewModel userModel);
        List<SettingsViewModel> GetAllSetting();
        IEnumerable<UserViewModel> GetUserSettings();
        SettingsDataModel GetSettingsWithUsers();
        void UpdateUser(UserViewModel userModel);

    }
}

