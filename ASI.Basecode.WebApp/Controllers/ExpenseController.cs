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
            var data = new ExpenseDataModel
            {
                ExpenseViewModel = expenses,
                CategoryViewModel = categories
            };
            //var data = _expensesService.GetAllExpenses();
            return View(data);
        }

        #region Get Methods
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var data = _expensesService.RetrieveExpenses(Id);
            return View(data);
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var data = _expensesService.RetrieveExpenses(Id);
            return View(data);
        }
        #endregion
        //public IActionResult Expenses(List<string> selectedCategories)
        //{
        //    var expenses = _expensesService.GetAllExpenses();

        //    if (selectedCategories != null && selectedCategories.Any())
        //    {
        //        // Attempt to convert selectedCategories to a list of integers
        //        var selectedCategoryIds = selectedCategories
        //            .Where(c => int.TryParse(c, out _))
        //            .Select(int.Parse)
        //            .ToList();

        //        // Filter expenses by integer CategoryId
        //        expenses = expenses.Where(e => selectedCategoryIds.Contains(e.CategoryId)).ToList();
        //    }

        //    return View(expenses);
        //}


        #region Post Methods

        [HttpPost]
        public IActionResult Create(ExpenseViewModel model)
        {
            TempData["SuccessMessage"] = "Expense added successfully.";
            _expensesService.AddExpenses(model, UserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(ExpenseViewModel model)
        {
            TempData["SuccessMessage"] = "Expense updated successfully.";
            _expensesService.UpdateExpenses(model, UserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PostDelete(int ExpenseId)
        {
            TempData["SuccessMessage"] = "Expense deleted successfully.";
            _expensesService.DeleteExpenses(ExpenseId);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
