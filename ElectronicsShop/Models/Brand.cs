using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace ElectronicsShop.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Country { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
