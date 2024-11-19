using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IExpenseService
    {
        void AddExpenses(ExpenseViewModel model, string userId);
        List<ExpenseViewModel> GetAllExpenses();
        IEnumerable<ExpenseViewModel> GetExpenseByUserId(string userId);
        ExpenseViewModel RetrieveExpenses(int id);
        void UpdateExpenses(ExpenseViewModel model, string userId);
        void DeleteExpenses(int id);
        IEnumerable<CategoryViewModel> GetCategories();
    }
}
