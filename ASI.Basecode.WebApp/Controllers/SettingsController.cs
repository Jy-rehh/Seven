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
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isPasswordChanged = _userService.ChangePassword(model);
                if (isPasswordChanged)
                {
                    TempData["SuccessMessage"] = "Password changed successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to change password. Please check your old password.";
                }
            }
            return RedirectToAction("Index");
        }
    }
}
