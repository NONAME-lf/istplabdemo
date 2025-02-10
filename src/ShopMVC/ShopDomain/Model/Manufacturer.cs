using System;
using System.Collections.Generic;

namespace ShopInfrastructure;

public partial class Manufacturer
{
    public int MnId { get; set; }

    public string? MnName { get; set; }

    public string? MnContactInfo { get; set; }

    public int? MnCountry { get; set; }

    public virtual Country? MnCountryNavigation { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
