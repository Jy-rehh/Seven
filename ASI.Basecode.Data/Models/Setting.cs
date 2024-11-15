using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Setting
    {
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Preference { get; set; }
    }
}
