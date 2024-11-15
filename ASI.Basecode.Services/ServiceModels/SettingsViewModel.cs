using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class SettingsViewModel
    {
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Preference { get; set; }
    }
    public class SettingsDataModel
    {
        public IEnumerable<SettingsViewModel> SettingsViewModel { get; set; }
        public IEnumerable<UserViewModel> UserViewModel { get; set; }
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }

    }
}
