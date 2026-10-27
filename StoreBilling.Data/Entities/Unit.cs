using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace StoreBilling.Data.Entities
{
    public class Unit : BaseEntity
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool AllowDecimal { get; set; }
    }
}