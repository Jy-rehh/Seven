using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using static ASI.Basecode.Resources.Constants.Enums;

public interface IUserService
{
    LoginResult AuthenticateUser(string userIdOrEmail, string password, ref User user);
    void AddUser(UserViewModel userModel);
    bool CheckEmailExists(string email);
    bool CheckUsernameExists(string username);
    UserViewModel GetUserByUserId(string userId);
    void UpdateUser(UserViewModel userModel);
    User GetUserById(string userId);
    bool VerifyPassword(User user, string inputPassword);
    bool ChangePassword(ChangePasswordViewModel model); // Use ViewModel for password change
}



