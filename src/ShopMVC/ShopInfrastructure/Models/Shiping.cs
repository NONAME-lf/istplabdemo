using System;
using System.Collections.Generic;

namespace ShopInfrastructure.Models;

public partial class Shiping
{
    public int ShId { get; set; }

    public string? ShAdress { get; set; }

    public int? CountryId { get; set; }

    public int? ShippingCompanyId { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    public virtual ShippingCompany? ShippingCompany { get; set; }
}
