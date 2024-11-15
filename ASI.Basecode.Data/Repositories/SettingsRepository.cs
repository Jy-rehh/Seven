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
    public class SettingsRepository : BaseRepository, ISettingsRepository
    {
        public SettingsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public void AddSetting(Setting model)
        {
            this.GetDbSet<Setting>().Add(model);
            UnitOfWork.SaveChanges();
        }
        public IEnumerable<Setting> GetAllSetting()
        {
            return this.GetDbSet<Setting>();
        }
        //public void UpdateSetting(Setting model)
        //{
        //    this.GetDbSet<Setting>().Update(model);
        //    UnitOfWork.SaveChanges();
        //}
        
    }
}






