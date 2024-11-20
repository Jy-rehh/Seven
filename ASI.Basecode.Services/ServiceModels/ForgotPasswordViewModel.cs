﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class ForgotPasswordViewModel

    {
        [Required]
        public string Email { get; set; }
        public bool EmailIsSent { get; set; }
    }
}
