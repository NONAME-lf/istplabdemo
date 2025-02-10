using System;
using System.Collections.Generic;

namespace ShopInfrastructure;

public partial class Category
{
    public int CgId { get; set; }

    public string? CgName { get; set; }

    public string? CgChildCategory { get; set; }

    public string? CgDescription { get; set; }

    public virtual ICollection<ProductCatergory> ProductCatergories { get; set; } = new List<ProductCatergory>();
}
