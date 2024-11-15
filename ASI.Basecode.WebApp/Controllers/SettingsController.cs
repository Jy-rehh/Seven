using ASI.Basecode.Services.Interfaces;
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
        private readonly ISettingsService _settingsService;
        private readonly IUserService _userService;
        public SettingsController(
                                IHttpContextAccessor httpContextAccessor,
                                ILoggerFactory loggerFactory,
                                IConfiguration configuration,
                                ISettingsService settingsService,
                                IUserService userService,
                                IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper) 
        {
            _settingsService = settingsService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var settingsData = _settingsService.GetSettingsWithUsers();

            // Assuming you need to set up ChangePasswordViewModel for the logged-in user
            var changePasswordViewModel = new ChangePasswordViewModel(); // Initialize a new instance

            settingsData.ChangePasswordViewModel = changePasswordViewModel; // Set the model for password change

            return View(settingsData);
        }

        [HttpPost]
        public IActionResult UpdatePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid form submission.";
                return RedirectToAction("Index");
            }

            var userId = User.Identity.Name;

            var user = _userService.GetUserByUserId(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Index");
            }

            if (user.Password != model.oldPassword)
            {
                TempData["Error"] = "The old password is incorrect.";
                return RedirectToAction("Index");
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                TempData["Error"] = "New password and confirmation do not match.";
                return RedirectToAction("Index");
            }

            user.Password = model.NewPassword;
            _userService.UpdateUser(user);

            TempData["Success"] = "Password updated successfully.";
            return RedirectToAction("Index");
        }
    }
}
