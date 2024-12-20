﻿using ASI.Basecode.Data;
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
        private readonly ICategoryRepository _categoryRepository;
        public ExpenseService(IExpenseRepository expensesRepository, ICategoryRepository categoryRepository, IMapper mapper, IConfiguration configuration)
        {
            _expensesRepository = expensesRepository;
            _mapper = mapper;
            _config = configuration;
            _categoryRepository = categoryRepository;
        }
        //public void AddExpenses(ExpenseViewModel model, string userId)
        //{
        //    var newExpenses = new Expense();
        //    newExpenses.CategoryId = model.CategoryId;
        //    newExpenses.UserId = userId;
        //    newExpenses.Title = model.Title;
        //    newExpenses.Amount = model.Amount;
        //    newExpenses.DateCreated = DateTime.Now;
        //    newExpenses.Description = model.Description;
        //    newExpenses.IsDeleted = false;
        //    _expensesRepository.AddExpenses(newExpenses);
        //}

        public void AddExpenses(ExpenseViewModel model, string userId)
        {
            var newExpense = new Expense
            {
                CategoryId = model.CategoryId,
                UserId = userId,
                Title = model.Title,
                Amount = model.Amount,
                DateCreated = DateTime.Now,
                Description = model.Description,
                IsDeleted = false
            };
            _expensesRepository.AddExpenses(newExpense);

            var category = _categoryRepository.GetAllCategory()
                                               .FirstOrDefault(c => c.CategoryId == model.CategoryId && !c.IsDeleted);

            if (category != null)
            {
                var totalExpensesInCategory = _expensesRepository.GetAllExpenses()
                                                                 .Count(e => e.CategoryId == model.CategoryId && !e.IsDeleted);

                category.TotalAmount = totalExpensesInCategory;

                _categoryRepository.UpdateCategory(category);
            }
        }


        public List<ExpenseViewModel> GetAllExpenses()
        {
            var categories = _categoryRepository.GetAllCategory().Select(s => new CategoryViewModel
            {
                CategoryId = s.CategoryId,
                Name = s.Name,
            });
            var serverUrl = _config.GetValue<string>("ServerUrl");
            var data = _expensesRepository.GetAllExpenses().Where(s => s.IsDeleted == true).Select(s => new ExpenseViewModel
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
                                                .Where(c => c.IsDeleted == false)
                                                .ToDictionary(c => c.CategoryId, c => c.Name);

            var expenses = _expensesRepository.GetAllExpenses().Where(e => e.UserId == userId && e.IsDeleted == false).Select(e => new ExpenseViewModel
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
            .Where(c => !c.IsDeleted)  // Exclude categories with Status = true
            .Select(s => new CategoryViewModel
            {
                CategoryId = s.CategoryId,
                Name = s.Name
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
        //public void UpdateExpenses(ExpenseViewModel model, string userId)
        //{
        //    var expense = _expensesRepository.GetAllExpenses().Where(x => x.ExpenseId.Equals(model.ExpenseId)).FirstOrDefault();
        //    model.DateCreated = expense.DateCreated;
        //    if (expense != null)
        //    {
        //        _mapper.Map(model, expense);
        //        expense.UserId = userId;
        //        _expensesRepository.UpdateExpenses(expense);
        //    }
        //}
        public void UpdateExpenses(ExpenseViewModel model, string userId)
        {
            var expense = _expensesRepository.GetAllExpenses()
                                              .FirstOrDefault(x => x.ExpenseId == model.ExpenseId && !x.IsDeleted);
            model.DateCreated = expense.DateCreated;
            if (expense != null)
            {
                var oldCategoryId = expense.CategoryId;

                _mapper.Map(model, expense);
                expense.UserId = userId;

                _expensesRepository.UpdateExpenses(expense);

                if (oldCategoryId != model.CategoryId)
                {
                    var oldCategory = _categoryRepository.GetAllCategory()
                                                          .FirstOrDefault(c => c.CategoryId == oldCategoryId && !c.IsDeleted);
                    if (oldCategory != null)
                    {
                        var totalExpensesInOldCategory = _expensesRepository.GetAllExpenses()
                                                                            .Count(e => e.CategoryId == oldCategoryId && !e.IsDeleted);
                        oldCategory.TotalAmount = totalExpensesInOldCategory;
                        _categoryRepository.UpdateCategory(oldCategory);
                    }

                    var newCategory = _categoryRepository.GetAllCategory()
                                                          .FirstOrDefault(c => c.CategoryId == model.CategoryId && !c.IsDeleted);
                    if (newCategory != null)
                    {
                        var totalExpensesInNewCategory = _expensesRepository.GetAllExpenses()
                                                                            .Count(e => e.CategoryId == model.CategoryId && !e.IsDeleted);
                        newCategory.TotalAmount = totalExpensesInNewCategory;
                        _categoryRepository.UpdateCategory(newCategory);
                    }
                }
            }
        }
        //public void DeleteExpenses(int Id)
        //{
        //    var expenses = _expensesRepository.GetAllExpenses().FirstOrDefault(x => x.ExpenseId == Id);
        //    if (expenses != null)
        //    {
        //        expenses.IsDeleted = true;
        //        _expensesRepository.UpdateExpenses(expenses);
        //    }
        //}

        public void DeleteExpenses(int Id)
        {
            var expense = _expensesRepository.GetAllExpenses().FirstOrDefault(x => x.ExpenseId == Id && !x.IsDeleted);
            if (expense != null)
            {
                expense.IsDeleted = true;
                _expensesRepository.UpdateExpenses(expense);

                var category = _categoryRepository.GetAllCategory()
                                                  .FirstOrDefault(c => c.CategoryId == expense.CategoryId && !c.IsDeleted);
                if (category != null)
                {
                    category.TotalAmount -= 1;
                    _categoryRepository.UpdateCategory(category);
                }
            }
        }

        // Ari ang pag call sa Id sa category nya makuha sa dropdown select sa expense nga add
        public IEnumerable<CategoryViewModel> GetCategories()
        {
            var categories = _categoryRepository.GetAllCategory()
                .Where(c => c.IsDeleted == false)
                .Select(c => new CategoryViewModel
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    DateCreated = c.DateCreated,
                    DateUpdated = c.DateUpdated,
                    TotalAmount = c.TotalAmount,
                });

            return categories;
        }

    }
}
