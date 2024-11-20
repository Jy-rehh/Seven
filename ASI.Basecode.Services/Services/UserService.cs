using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public LoginResult AuthenticateUser(string userIdOrEmail, string password, ref User user)
        {
            var passwordKey = PasswordManager.EncryptPassword(password);

            bool isEmail = userIdOrEmail.Contains("@");
            user = isEmail
                   ? _repository.GetUsers().FirstOrDefault(x => x.Email == userIdOrEmail && x.Password == passwordKey)
                   : _repository.GetUsers().FirstOrDefault(x => x.UserId == userIdOrEmail && x.Password == passwordKey);

            return user != null ? LoginResult.Success : LoginResult.Failed;
        }

        public void AddUser(UserViewModel UserModel)
        {
            var user = new User();
            _mapper.Map(UserModel, user);
            user.Email = UserModel.Email;
            user.Password = PasswordManager.EncryptPassword(UserModel.Password);
            user.CreatedTime = DateTime.Now;
            user.UpdatedTime = DateTime.Now;
            user.CreatedBy = Environment.UserName;
            user.UpdatedBy = Environment.UserName;
            user.Preference = "Php";

            _repository.AddUser(user);
        }

        public bool CheckEmailExists(string email)
        {
            return _repository.GetUsers().Any(u => u.Email == email);
        }
        public bool CheckUsernameExists(string username)
        {
            return _repository.GetUsers().Any(u => u.UserId == username);
        }


        public UserViewModel GetUserByUserId(string userId)
        {
            var user = _repository.GetUserByUserId(userId);

            return user != null ? _mapper.Map<UserViewModel>(user) : null;
        }

        public void UpdateUser(UserViewModel userModel)
        {
            var user = _repository.GetUsers().FirstOrDefault(u => u.Id == userModel.Id);

            if (user != null)
            {
                user.Name = userModel.Name;
                user.Email = userModel.Email;
                user.Preference = userModel.Preference;

                _repository.UpdateUser(user);
            }
        }
        public bool ChangePassword(ChangePasswordViewModel model)
        {
            var user = _repository.GetUsers().FirstOrDefault(u => u.UserId == model.UserId);
            if (user != null && user.Password == PasswordManager.EncryptPassword(model.OldPassword))
            {
                user.Password = PasswordManager.EncryptPassword(model.NewPassword);
                _repository.UpdateUser(user);
                return true;
            }
            return false;
        }
        public static bool VerifyPassword(string storedPassword, string inputPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedPassword);
        }
    }
}