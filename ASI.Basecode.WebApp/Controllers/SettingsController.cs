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
using Serilog.Core;
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
        public IActionResult Edit(UserViewModel model)
        {
            var existingUser = _userService.GetUserByUserId(UserId);

            if (existingUser != null)
            {
                existingUser.Name = model.Name;
                existingUser.Email = model.Email;
                existingUser.Preference = model.Preference;

                // Save the changes
                _userService.UpdateUser(existingUser);
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword, string userId)
        {
            // Validate the new password and confirm password
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View(); // Return the view with error
            }

            // Create the ChangePasswordViewModel to pass to the service
            var changePasswordModel = new ChangePasswordViewModel
            {
                UserId = userId,  // The UserId is passed from the form
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            // Call the service to change the password
            bool result = _userService.ChangePassword(changePasswordModel);

            if (result)
            {
                // Password changed successfully
                TempData["SuccessMessage"] = "Password changed successfully!";
                return RedirectToAction("Index"); // Or another redirect, e.g., to the profile page
            }
            else
            {
                // If old password is incorrect
                ModelState.AddModelError("", "Old password is incorrect.");
                return View(); // Return the view with error
            }
        }

    }
}
