using System;
using System.Collections.Generic;

namespace ShopInfrastructure.Models;

public partial class Cart
{
    public int CtId { get; set; }

    public int? CtQuantity { get; set; }

    public int? CtPrice { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<ProductCart> ProductCarts { get; set; } = new List<ProductCart>();

    public virtual User? User { get; set; }
}
