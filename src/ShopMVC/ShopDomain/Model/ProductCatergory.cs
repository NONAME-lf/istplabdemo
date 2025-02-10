using System;
using System.Collections.Generic;
using ShopDomain.Model;

namespace ShopInfrastructure;

public partial class ProductCatergory
{
    public int PctId { get; set; }

    public int? PctProductId { get; set; }

    public int? PctCategoryId { get; set; }

    public virtual Category? PctCategory { get; set; }

    public virtual Product? PctProduct { get; set; }
}
