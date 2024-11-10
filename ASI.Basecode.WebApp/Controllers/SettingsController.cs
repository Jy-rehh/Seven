using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Models;
using System.Threading.Tasks;

namespace ASI.Basecode.WebApp.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        // GET: Retrieve current user settings
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var settings = await _settingsService.GetUserSettings(User.Identity.Name);
            return View(settings);
        }

        // POST: Update user settings
        [HttpPost]
        public async Task<IActionResult> Update(SettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            await _settingsService.UpdateUserSettings(User.Identity.Name, model);
            TempData["SuccessMessage"] = "Settings updated successfully.";
            return RedirectToAction("Index");
        }

        // POST: Reset user settings to default
        [HttpPost]
        public async Task<IActionResult> Reset()
        {
            await _settingsService.ResetUserSettings(User.Identity.Name);
            TempData["SuccessMessage"] = "Settings reset to default.";
            return RedirectToAction("Index");
        }
    }
}
