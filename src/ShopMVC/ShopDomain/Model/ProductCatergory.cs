using System;
using System.Collections.Generic;

namespace ShopDomain.Model;

public partial class ProductCatergory : Entity
{
    //public int PctId { get; set; }

    public int? PctProductId { get; set; }

    public int? PctCategoryId { get; set; }

    public virtual Category? PctCategory { get; set; }

    public virtual Product? PctProduct { get; set; }
}
