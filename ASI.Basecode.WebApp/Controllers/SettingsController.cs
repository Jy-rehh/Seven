using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Models;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace ASI.Basecode.WebApp.Controllers
{
    public class SettingsController : ControllerBase<SettingsController>
    {
        private readonly ISettingsService _settingsService;
        // Constructor to inject dependencies like settings service
        public SettingsController(
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            ISettingsService settingsService,
            IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _settingsService = settingsService;
        }

        // Action to view and edit settings (GET request)
        public IActionResult Index()
        {
            // Fetch current settings using the service
            var settings = _settingsService.GetSettings();

            // Map settings to the view model
            var settingsViewModel = new SettingsViewModel
            {
                Username = settings.Username,
                Field1 = settings.Field1,
                Field2 = settings.Field2
            };

            return View(settingsViewModel);
        }

        // Action to save settings (POST request)
        [HttpPost]
        public IActionResult SaveSettings(SettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Save the updated settings using the service
                _settingsService.SaveSettings(model.Username, model.Field1, model.Field2);

                // Display success message
                TempData["Message"] = "Settings saved successfully!";
                return RedirectToAction("Index");
            }

            // If validation fails, return to the settings page with validation errors
            return View("Index", model);
        }
    }
}


