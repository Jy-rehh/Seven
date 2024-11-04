using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class ExpenseRepository : BaseRepository, IExpenseRepository
    {
        public ExpenseRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public void AddExpenses(Expense model)
        {
            this.GetDbSet<Expense>().Add(model);
            UnitOfWork.SaveChanges();
        }
        public IEnumerable<Expense> GetAllExpenses()
        {
            return this.GetDbSet<Expense>();
        }
        public void UpdateExpenses(Expense model)
        {
            this.GetDbSet<Expense>().Update(model);
            UnitOfWork.SaveChanges();
        }
        public void DeleteExpenses(Expense model)
        {
            this.GetDbSet<Expense>().Remove(model);
            UnitOfWork.SaveChanges();
        }
    }
}
