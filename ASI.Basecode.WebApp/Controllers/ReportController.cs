using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
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
        private readonly IExpenseService _expenseService;

        public ReportController(
            IHttpContextAccessor httpContextAccessor, 
            ILoggerFactory loggerFactory, 
            IConfiguration configuration,
            IReportService reportService,
            IExpenseService expenseService,
            IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _reportService = reportService;
            _expenseService = expenseService;
        }

        // GET: /Report/Index
        public IActionResult Index()
        {
            var expenses = _expenseService.GetExpenseByUserId(UserId);
            var categories = _expenseService.GetCategories();
            var data = new ExpenseDataModel
            {
                ExpenseViewModel = expenses,
                CategoryViewModel = categories
            };
            return View(data);
        }
    }
}
