﻿using System;
using System.Collections.Generic;

namespace TemplatesSample.Models
{
    public partial class CustomerSetting
    {
        public string CustomerId { get; set; }
        public string Setting { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
