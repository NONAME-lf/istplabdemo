using System;
using System.Collections.Generic;

namespace ShopInfrastructure.Models;

public partial class Country
{
    public int CoId { get; set; }

    public string? CoName { get; set; }

    public virtual ICollection<Manufacturer> Manufacturers { get; set; } = new List<Manufacturer>();

    public virtual ICollection<Shiping> Shipings { get; set; } = new List<Shiping>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
