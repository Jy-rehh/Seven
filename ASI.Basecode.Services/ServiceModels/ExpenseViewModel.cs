using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class ExpenseViewModel
    {
        public int ExpenseId { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public float Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
    }
}
