using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers
{
    public class ReportController : ControllerBase<ReportController>
    {
        private readonly IReportService _reportService;

        public ReportController(
            IHttpContextAccessor httpContextAccessor, 
            ILoggerFactory loggerFactory, 
            IConfiguration configuration,
            IReportService reportService,
            IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _reportService = reportService;

        }

        // GET: /Report/Index
        public IActionResult Index()
        {
            return View();
        }
    }
}
