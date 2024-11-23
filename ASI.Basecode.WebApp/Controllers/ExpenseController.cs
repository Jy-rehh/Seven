using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class ExpenseController : ControllerBase<ExpenseController>
{
    private readonly IExpenseService _expensesService;

    public ExpenseController(
        IHttpContextAccessor httpContextAccessor,
    ILoggerFactory loggerFactory,
        IConfiguration configuration,
        IExpenseService expensesService,
        IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _expensesService = expensesService;
    }

    public IActionResult Index()
    {
        var expenses = _expensesService.GetExpenseByUserId(UserId);
        var categories = _expensesService.GetCategories();

        // Retrieve user's currency preference
        var currencySymbol = GetCurrencyPreference(UserId) ?? "₱"; // Default to PHP

        var data = new ExpenseDataModel
        {
            ExpenseViewModel = expenses,
            CategoryViewModel = categories
        };

        // Pass the currency symbol to the view
        ViewBag.CurrencySymbol = currencySymbol;

        return View(data);
    }

    #region Helper Methods
    private string GetCurrencyPreference(string userId)
    {
        // Replace this with actual logic to retrieve the user's preference
        // For example, from the database or session:
        // return _userService.GetCurrencyPreferenceByUserId(userId);
        return "₱"; // Default currency
    }
    #endregion

    #region Get Methods
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(ExpenseViewModel model)
    {
        TempData["SuccessMessage"] = "Expense added successfully.";
        _expensesService.AddExpenses(model, UserId);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Edit(int id)
    {
        var data = _expensesService.RetrieveExpenses(id);
        return View(data);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var data = _expensesService.RetrieveExpenses(id);
        return View(data);
    }
    #endregion
}