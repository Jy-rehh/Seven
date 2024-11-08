using ASI.Basecode.Data;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expensesRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public ExpenseService(IExpenseRepository expensesRepository, IMapper mapper, IConfiguration configuration)
        {
            _expensesRepository = expensesRepository;
            _mapper = mapper;
            _config = configuration;
        }
        public void AddExpenses(ExpenseViewModel model, string userId)
        {
            var newExpenses = new Expense();
            newExpenses.CategoryId = model.CategoryId;
            newExpenses.UserId = userId;
            newExpenses.Title = model.Title;
            newExpenses.Amount = model.Amount;
            newExpenses.DateCreated = DateTime.Now;
            newExpenses.Description = model.Description;
            _expensesRepository.AddExpenses(newExpenses);

        }
        public List<ExpenseViewModel> GetAllExpenses()
        {
            var serverUrl = _config.GetValue<string>("ServerUrl");
            var data = _expensesRepository.GetAllExpenses().Select(s => new ExpenseViewModel
            {
                ExpenseId = s.ExpenseId,
                CategoryId = s.CategoryId,
                UserId = s.UserId,
                Title = s.Title,
                Amount = (float)s.Amount,
                DateCreated = s.DateCreated,
                Description = s.Description,
            }).ToList();
            return data;
        }
        public ExpenseViewModel RetrieveExpenses(int Id)
        {
            var expenses = _expensesRepository.GetAllExpenses().Where(x => x.ExpenseId.Equals(Id)).Select(s => new ExpenseViewModel
            {
                ExpenseId = s.ExpenseId,
                CategoryId = s.CategoryId,
                UserId = s.UserId,
                Title = s.Title,
                Amount = (float)s.Amount,
                DateCreated = s.DateCreated,
                Description = s.Description,
            }).FirstOrDefault();
            return expenses;
        }
        public void UpdateExpenses(ExpenseViewModel model, string userId)
        {
            var expense = _expensesRepository.GetAllExpenses().Where(x => x.ExpenseId.Equals(model.ExpenseId)).FirstOrDefault();
            model.DateCreated = expense.DateCreated;
            if (expense != null)
            {
                _mapper.Map(model, expense);
                expense.UserId = userId;
                _expensesRepository.UpdateExpenses(expense);
            }
        }
        public void DeleteExpenses(int Id)
        {
            var expenses = _expensesRepository.GetAllExpenses().Where(x => x.ExpenseId.Equals(Id)).FirstOrDefault();
            if (expenses != null)
            {
                _expensesRepository.DeleteExpenses(expenses);
            }
        }
    }
}
