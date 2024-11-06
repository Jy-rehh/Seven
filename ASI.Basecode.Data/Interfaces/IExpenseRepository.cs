using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IExpenseRepository
    {
        void AddExpenses(Expense model);
        IEnumerable<Expense> GetAllExpenses();
        void UpdateExpenses(Expense model);
        void DeleteExpenses(Expense model);
    }
}
