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

        // GET: /Report/Index
        public IActionResult Index()
        {
            return View();
        }
    }
}


