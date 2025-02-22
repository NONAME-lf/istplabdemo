using System;
using System.Collections.Generic;

namespace ShopDomain.Model;

public partial class ShippingCompany : Entity
{
    //public int ScId { get; set; }

    public int? ScName { get; set; }

    public decimal? ScPricing { get; set; }

    public string? ScAvgTimeNeed { get; set; }

    public virtual ICollection<Shiping> Shipings { get; set; } = new List<Shiping>();
}
