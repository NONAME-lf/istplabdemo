using System;
using System.Collections.Generic;
using ShopDomain.Model;

namespace ShopInfrastructure;

public partial class ProductOrder : Entity
{
    //public int PoId { get; set; }

    public int? PoProductId { get; set; }

    public int? PoOrderId { get; set; }

    public decimal? PoPrice { get; set; }

    public int? PoQuantity { get; set; }

    public virtual Order? PoOrder { get; set; }

    public virtual Product? PoProduct { get; set; }
}
