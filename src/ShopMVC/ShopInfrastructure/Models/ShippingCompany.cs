using System;
using System.Collections.Generic;

namespace ShopInfrastructure.Models;

public partial class ShippingCompany
{
    public int ScId { get; set; }

    public string? ScName { get; set; }

    public decimal? ScPricing { get; set; }

    public string? ScAvgTimeNeed { get; set; }

    public virtual ICollection<Shiping> Shipings { get; set; } = new List<Shiping>();
}
