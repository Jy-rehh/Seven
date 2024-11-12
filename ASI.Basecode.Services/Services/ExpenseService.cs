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
        private readonly AsiBasecodeDBContext _dbContext;
        private readonly ICategoryRepository _categoryRepository;
        public ExpenseService(AsiBasecodeDBContext asiBasecodeDBContext ,IExpenseRepository expensesRepository, ICategoryRepository categoryRepository, IMapper mapper, IConfiguration configuration)
        {
            _expensesRepository = expensesRepository;
            _mapper = mapper;
            _config = configuration;
            _categoryRepository = categoryRepository;
            _dbContext = asiBasecodeDBContext;
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
            newExpenses.Status = false;
            _expensesRepository.AddExpenses(newExpenses);

        }
        public List<ExpenseViewModel> GetAllExpenses()
        {
            var categories = _categoryRepository.GetAllCategory().Select(s => new CategoryViewModel
            {
                CategoryId = s.CategoryId,
                Name = s.Name,
            });
            var serverUrl = _config.GetValue<string>("ServerUrl");
            var data = _expensesRepository.GetAllExpenses().Where(s => s.Status == false).Select(s => new ExpenseViewModel
            {
                ExpenseId = s.ExpenseId,
                CategoryId = s.CategoryId,
                CategoryName = categories.FirstOrDefault(c => c.CategoryId == s.CategoryId)?.Name,
                UserId = s.UserId,
                Title = s.Title,
                Amount = (float)s.Amount,
                DateCreated = s.DateCreated,
                Description = s.Description,
            }).ToList();
            return data;
        }
        public IEnumerable<ExpenseViewModel> GetExpenseByUserId(string userId)
        {
            var categories = _categoryRepository.GetAllCategory()
                                                .Where(c => c.Status == false)
                                                .ToDictionary(c => c.CategoryId, c => c.Name);

            var expenses = _expensesRepository.GetAllExpenses().Where(e => e.UserId == userId).Select(e => new ExpenseViewModel
            {
                ExpenseId = e.ExpenseId,
                CategoryId = e.CategoryId,
                CategoryName = categories.ContainsKey(e.CategoryId) ? categories[e.CategoryId] : "Unknown",
                UserId = e.UserId,
                Title = e.Title,
                Amount = (float)e.Amount,
                DateCreated = e.DateCreated,
                Description = e.Description,
            });
            return expenses;
        }

        public ExpenseViewModel RetrieveExpenses(int Id)
        {
            var categories = _categoryRepository.GetAllCategory()
            .Where(c => !c.Status)  // Exclude categories with Status = true
            .Select(s => new CategoryViewModel
            {
                CategoryId = s.CategoryId,
                Name = s.Name,
            });
            var expenses = _expensesRepository.GetAllExpenses().Where(x => x.ExpenseId.Equals(Id)).Select(s => new ExpenseViewModel
            {
                ExpenseId = s.ExpenseId,
                //CategoryId = categories.Where(x => x.CategoryId.Equals(x.CategoryId)).Select(s => s.Name).FirstOrDefault(),
                CategoryId = s.CategoryId,
                CategoryName = categories.FirstOrDefault(c => c.CategoryId == s.CategoryId)?.Name,
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
            var expenses = _expensesRepository.GetAllExpenses().FirstOrDefault(x => x.ExpenseId == Id);
            if (expenses != null)
            {
                expenses.Status = true;
                _expensesRepository.UpdateExpenses(expenses);
            }
        }
        // Ari ang pag call sa Id sa category nya makuha sa dropdown select sa expense nga add
        public IEnumerable<CategoryViewModel> GetCategories()
        {
            var categories = _categoryRepository.GetAllCategory()
                .Where(c => c.Status == false) // Only include categories with Status = false
                .Select(c => new CategoryViewModel
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name
                });

            return categories;
        }

    }
}
