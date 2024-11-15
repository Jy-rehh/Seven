using ASI.Basecode.Data;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        public SettingsService(IUserRepository userRepository, ISettingsRepository settingsRepository, IMapper mapper, IConfiguration config)
        {
            _settingsRepository = settingsRepository;
            _mapper = mapper;
            _config = config;
            _userRepository = userRepository;
        }
        public List<SettingsViewModel> GetAllSetting()
        {
            var data = _settingsRepository.GetAllSetting().Select(s => new SettingsViewModel
            {
                UserId = s.UserId,
                DateCreated = s.DateCreated,
                DateUpdated = s.DateUpdated,
                Preference = s.Preference

            }).ToList();

            return data;
        }

        public SettingsDataModel GetSettingsWithUsers()
        {
            var users = _userRepository.GetUsers().Select(user => new UserViewModel
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email
            }).ToList();

            var settings = _settingsRepository.GetAllSetting().Select(setting => new SettingsViewModel
            {
                UserId = setting.UserId,
                DateCreated = setting.DateCreated,
                DateUpdated = setting.DateUpdated,
                Preference = setting.Preference
            }).ToList();

            foreach (var setting in settings)
            {
                var matchedUser = users.FirstOrDefault(u => u.UserId == setting.UserId.ToString());
                if (matchedUser != null)
                {
                    // Optionally enrich data or log connections
                }
            }

            return new SettingsDataModel
            {
                UserViewModel = users,
                SettingsViewModel = settings
            };
        }
        public void UpdateUser(UserViewModel userModel)
        {
            var user = _userRepository.GetUsers().FirstOrDefault(u => u.UserId == userModel.UserId);
            if (user != null)
            {
                user.Password = userModel.Password; 
                user.Name = userModel.Name; 
                _userRepository.UpdateUser(user); 
            }
        }


        //public void UpdateSettings(SettingsViewModel settingsModel, UserViewModel userModel) 
        //{
        //    var user = _userRepository.GetUsers().Where(x => x.Id.Equals(userModel.Id)).FirstOrDefault();

        //    var setting = _settingsRepository.GetAllSetting().Where(x => x.UserId.Equals(settingsModel.UserId)).FirstOrDefault();
        //    settingsModel.DateCreated = setting.DateCreated;
        //    if (setting != null)
        //    {
        //        _mapper.Map(settingsModel, setting);
        //        _settingsRepository.UpdateSettings(setting, user);
        //    }
        //}

        public void UpdateSettings(ExpenseViewModel expenseModel, UserViewModel userModel)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<UserViewModel> GetUserSettings()
        {
            var settings = _userRepository.GetUsers().Select(c => new UserViewModel
            {
                Name = c.Name,
                UserId = c.UserId,
                Email = c.Email,
            });
            return settings;
        }
    }
}


