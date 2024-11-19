using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }

    public class CategoryDataModel
    {
        public IEnumerable<ExpenseViewModel> ExpenseViewModel { get; set; }
        public IEnumerable<CategoryViewModel> CategoryViewModel { get; set; }
    }
}
