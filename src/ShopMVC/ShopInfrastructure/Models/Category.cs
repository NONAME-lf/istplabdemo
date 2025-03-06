using System;
using System.Collections.Generic;

namespace ShopInfrastructure.Models;

public partial class Category
{
    public int CgId { get; set; }

    public string? CgName { get; set; }

    public string? CgChildCategory { get; set; }

    public string? CgDescription { get; set; }

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
