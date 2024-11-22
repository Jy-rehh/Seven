using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : ControllerBase<HomeController>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>
        /// 
        private readonly IExpenseService _expenseService;
        public HomeController(IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IConfiguration configuration,
                              IExpenseService expenseService,
                              IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _expenseService = expenseService;
        }

        /// <summary>
        /// Returns Home View.
        /// </summary>
        /// <returns> Home View </returns>
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

        public IActionResult DashBoard()
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
