using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class SettingsController : ControllerBase<SettingsController>
    {
        private readonly IUserService _userService;
        public SettingsController(
                                IHttpContextAccessor httpContextAccessor,
                                ILoggerFactory loggerFactory,
                                IConfiguration configuration,
                                IUserService userService,
                                IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper) 
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            string currentUserId = UserId;

            var userViewModel = _userService.GetUserByUserId(currentUserId);


            return View(userViewModel);
        }
        [HttpPost]
        public IActionResult UpdateUserDetails([FromBody] UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByUserId(User.Identity.Name);
                if (user != null)
                {
                    user.Name = model.Name;
                    user.Email = model.Email;
                    user.UserId = model.UserId;
                    user.Preference = model.Preference;

                    _userService.UpdateUser(user);

                    return Json(new { success = true, message = "User details updated successfully." });
                }
            }
            return Json(new { success = false, message = "Invalid data." });
        }

        [HttpPost]
        public IActionResult ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByUserId(User.Identity.Name);
                if (user != null && user.Password == PasswordManager.EncryptPassword(model.OldPassword))
                {
                    if (model.NewPassword == model.ConfirmPassword)
                    {
                        user.Password = PasswordManager.EncryptPassword(model.NewPassword);
                        _userService.UpdateUser(user);

                        return Json(new { success = true, message = "Password changed successfully." });
                    }
                    return Json(new { success = false, message = "Passwords do not match." });
                }
                return Json(new { success = false, message = "Old password is incorrect." });
            }
            return Json(new { success = false, message = "Invalid data." });
        }
    }
}
