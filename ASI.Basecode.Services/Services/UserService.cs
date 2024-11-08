﻿using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
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

        public void AddUser(UserViewModel model)
        {
            if (CheckEmailExists(model.Email))
            {
                throw new InvalidOperationException("This email is already in use.");
            }

            var user = new User();
            _mapper.Map(model, user);
            user.Email = model.Email;
            user.Password = PasswordManager.EncryptPassword(model.Password);
            user.CreatedTime = DateTime.Now;
            user.UpdatedTime = DateTime.Now;
            user.CreatedBy = Environment.UserName;
            user.UpdatedBy = Environment.UserName;

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
    }
}

