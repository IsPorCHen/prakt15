using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ElectronicsShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(3,2)")]
        public decimal Rating { get; set; } = 0;

        public string? ImagePath { get; set; }

        public int CategoryId { get; set; }
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();

        [NotMapped]
        public List<Tag> Tags => ProductTags.Select(pt => pt.Tag).ToList();

        [NotMapped]
        public bool IsLowStock => Quantity < 10;

        [NotMapped]
        public string DisplayName => $"{Brand?.Name ?? ""} {Name}";

        [NotMapped]
        public string PriceFormatted => $"{Price:N2} ₽";

        [NotMapped]
        public string QuantityText => $"В наличии: {Quantity} шт.";

        [NotMapped]
        public string RatingText => Rating > 0 ? $"{Rating:F1} ★" : "Нет оценки";

        [NotMapped]
        public string RatingStars => GetRatingStars();

        private string GetRatingStars()
        {
            if (Rating == 0) return "☆☆☆☆☆";

            int fullStars = (int)Math.Floor(Rating);
            int halfStar = Rating - fullStars >= 0.3m ? 1 : 0;
            int emptyStars = 5 - fullStars - halfStar;

            return new string('★', fullStars) +
                   (halfStar > 0 ? "½" : "") +
                   new string('☆', emptyStars);
        }
    }
}