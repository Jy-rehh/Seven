using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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
            var data = _expensesService.GetAllExpenses();
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

        #region Post Methods

        [HttpPost]
        public IActionResult Create(ExpenseViewModel model)
        {
            _expensesService.AddExpenses(model, UserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PostDelete(int ExpensesId)
        {
            _expensesService.DeleteExpenses(ExpensesId);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
