using System;
using System.Collections.Generic;

namespace ElectronicsShop.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string? ImagePath { get; set; }

    public int CategoryId { get; set; }

    public int BrandId { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public bool IsLowStock => Quantity < 10;
    public string DisplayName => $"{Brand?.Name} {Name}";
    public string PriceFormatted => $"{Price:N2} ₽";
    public string QuantityText => $"В наличии: {Quantity} шт.";
}
