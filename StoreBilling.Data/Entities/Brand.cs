using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBilling.Data.Entities
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}