﻿using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using static ASI.Basecode.Resources.Constants.Enums;

public interface IUserService
{
    LoginResult AuthenticateUser(string userIdOrEmail, string password, ref User user);
    void AddUser(UserViewModel model);
    bool CheckEmailExists(string email);
    bool CheckUsernameExists(string username);
}

