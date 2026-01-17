using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ElectronicsShop.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();

        [NotMapped]
        public List<Product> Products => ProductTags.Select(pt => pt.Product).ToList();
    }
}