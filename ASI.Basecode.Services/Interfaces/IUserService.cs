using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System.Threading.Tasks;
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
    bool ChangePassword(ChangePasswordViewModel model); // Use ViewModel for password change


    Task<User> GetUserByEmail(string email);
    Task<bool> UpdateUserPassword(User user);
    //Task<User> GetUserByTokenAsync(string token);
    Task ClearResetTokenAsync(int userId);
    Task<bool> SetPasswordResetTokenAsync(string email);
    User GetUserByToken(string token);

    // helper
    bool ChangePasswordWithoutOldPassword(ResetPasswordViewModel model);

}



