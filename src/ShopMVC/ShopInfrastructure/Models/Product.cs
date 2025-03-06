using System;
using System.Collections.Generic;

namespace ShopInfrastructure.Models;

public partial class Product
{
    public int PdId { get; set; }

    public string? PdName { get; set; }

    public decimal? PdPrice { get; set; }

    public string? PdMeasurements { get; set; }

    public int? PdQuantity { get; set; }

    public double? PdDiscount { get; set; }

    public string? PdAbout { get; set; }

    public int? ManufacturerId { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductCart> ProductCarts { get; set; } = new List<ProductCart>();

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public virtual ICollection<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();
}
