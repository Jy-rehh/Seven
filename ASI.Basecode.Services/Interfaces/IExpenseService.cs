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
        ExpenseViewModel RetrieveExpenses(int id);
        void DeleteExpenses(int id);
    }
}
