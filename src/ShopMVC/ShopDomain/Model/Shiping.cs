using System;
using System.Collections.Generic;
using ShopDomain.Model;

namespace ShopInfrastructure;

public partial class Shiping : Entity
{
    //public int ShId { get; set; }

    public string? ShAdress { get; set; }

    public int? ShCountryId { get; set; }

    public int? ShShippingCompanyId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    public virtual Country? ShCountry { get; set; }

    public virtual ShippingCompany? ShShippingCompany { get; set; }
}
