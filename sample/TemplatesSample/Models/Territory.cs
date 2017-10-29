using System;
using System.Collections.Generic;

namespace TemplatesSample.Models
{
    public partial class Territory
    {
        public Territory()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritories>();
        }

        public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }

        public ICollection<EmployeeTerritories> EmployeeTerritories { get; set; }
    }
}
