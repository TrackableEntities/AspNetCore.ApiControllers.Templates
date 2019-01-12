﻿using System;
using System.Collections.Generic;

namespace TemplatesSample.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Order = new HashSet<Order>();
        }

        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public virtual CustomerSetting CustomerSetting { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
