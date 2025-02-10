using System;
using System.Collections.Generic;

namespace ShopInfrastructure;

public partial class Cart
{
    public int CtId { get; set; }

    public int? CtQuantity { get; set; }

    public int? CtPrice { get; set; }

    public int? CtUserId { get; set; }

    public virtual User? CtUser { get; set; }

    public virtual ICollection<ProductCart> ProductCarts { get; set; } = new List<ProductCart>();
}
