using ASI.Basecode.Data;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly AsiBasecodeDBContext _dbContext;

        public UserService(AsiBasecodeDBContext dBContext, IEmailService emailService, IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _emailService = emailService;
            _dbContext = dBContext;
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
            if (user != null)
            {
                user.Password = PasswordManager.EncryptPassword(model.NewPassword);
                _repository.UpdateUser(user);
                return true;
            }
            return false;
        }


        // Fetch the user by Id
        public User GetUserById(string userId)
        {
            var user = _repository.GetUsers().FirstOrDefault(u => u.UserId == userId); // Assuming 'UserId' is the identifier
            return user;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await Task.FromResult(_repository.GetUsers().FirstOrDefault(u => u.Email == email));
        }
        public async Task<bool> SetPasswordResetTokenAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false; // Return false if the email is null or empty
            }

            var user = _repository.GetUsers().FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return false; // Return false if the user is not found
            }

            var token = Guid.NewGuid().ToString();
            user.Token = token;
            user.TokenExpiry = DateTime.Now.AddHours(1);

            _repository.UpdateUser(user);

            var resetLink = $"https://localhost:50885/Account/ResetPassword?token={token}";
            await _emailService.SendEmailAsync(user.Email, "Reset Password", $"Click the link to reset your password: {resetLink}");

            return true;
        }

        public async Task SaveResetTokenAsync(int userId, string resetToken)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.Token = resetToken;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ClearResetTokenAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.Token = null;
                user.TokenExpiry = null;
                await _dbContext.SaveChangesAsync();
            }
        }
        public User GetUserByToken(string token)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Token == token);
        }

        public bool ChangePasswordWithoutOldPassword(ResetPasswordViewModel model)
        {
            var user = _repository.GetUsers().FirstOrDefault(u => u.UserId == model.UserId);
            if (user != null)
            {
                user.Password = PasswordManager.EncryptPassword(model.Password);
                _repository.UpdateUser(user);
                return true;
            }
            return false;
        }
    }
}
