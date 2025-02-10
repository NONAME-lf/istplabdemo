using System;
using System.Collections.Generic;
using ShopDomain.Model;

namespace ShopInfrastructure;

public partial class ProductCart
{
    public int PcId { get; set; }

    public int? PcProduct { get; set; }

    public int? PcCart { get; set; }

    public int? PcQuantity { get; set; }

    public decimal? PcPrice { get; set; }

    public virtual Cart? PcCartNavigation { get; set; }

    public virtual Product? PcProductNavigation { get; set; }
}
