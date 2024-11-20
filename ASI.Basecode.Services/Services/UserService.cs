using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using BCrypt.Net;
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
            var passwordKey = PasswordManager.EncryptPassword(password); // This should be replaced by BCrypt

            bool isEmail = userIdOrEmail.Contains("@");
            user = isEmail
                   ? _repository.GetUsers().FirstOrDefault(x => x.Email == userIdOrEmail && x.Password == passwordKey)
                   : _repository.GetUsers().FirstOrDefault(x => x.UserId == userIdOrEmail && x.Password == passwordKey);

            return user != null ? LoginResult.Success : LoginResult.Failed;
        }

        public void AddUser(UserViewModel userModel)
        {
            var user = new User();
            _mapper.Map(userModel, user);
            user.Email = userModel.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password); // Hash the password properly
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
            if (user != null && VerifyPassword(user, model.OldPassword))
            {
                // Rehash the new password if needed
                user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                _repository.UpdateUser(user); // Save the updated user in the repository
                return true;
            }
            return false;
        }

        // Modified to match interface: Accept User object, not just userId
        public bool VerifyPassword(User user, string inputPassword)
        {
            if (user == null) return false;

            try
            {
                // Try verifying the password with the BCrypt library
                if (BCrypt.Net.BCrypt.Verify(inputPassword, user.Password))
                {
                    // If the password is correct, check if the hash needs to be rehashed
                    if (IsPasswordRehashedNeeded(user.Password))
                    {
                        user.Password = BCrypt.Net.BCrypt.HashPassword(inputPassword); // Rehash if needed
                        _repository.UpdateUser(user); // Save the updated user
                    }
                    return true;
                }
            }
            catch (BCrypt.Net.SaltParseException ex)
            {
                // Handle invalid salt version error by attempting to rehash the password
                Console.WriteLine("Salt parsing exception: " + ex.Message);

                // Attempt rehashing the password and updating the stored value
                if (inputPassword != null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(inputPassword);
                    _repository.UpdateUser(user); // Save the updated user
                    return true;
                }
            }
            return false;
        }

        private bool IsPasswordRehashedNeeded(string storedPasswordHash)
        {
            // Example check for the salt version (the hash format should start with '$2a$')
            return !storedPasswordHash.StartsWith("$2a$");
        }


        // Fetch the user by Id
        public User GetUserById(string userId)
        {
            var user = _repository.GetUsers().FirstOrDefault(u => u.UserId == userId); // Assuming 'UserId' is the identifier
            return user;
        }
    }
}
