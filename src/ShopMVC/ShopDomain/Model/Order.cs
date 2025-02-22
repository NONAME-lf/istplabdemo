using System;
using System.Collections.Generic;

namespace ShopDomain.Model;

public partial class Order : Entity
{
    //public int OdId { get; set; }

    public int? OdUser { get; set; }

    public decimal? OdTotal { get; set; }

    public double? OdDiscount { get; set; }

    public string? OdPayment { get; set; }

    public string? OdNotes { get; set; }

    public int? ReceiptId { get; set; }

    public int? ProtuctId { get; set; }

    public int? ShippingId { get; set; }

    public virtual User? OdUserNavigation { get; set; }

    public virtual ICollection<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();

    public virtual Product? Protuct { get; set; }

    public virtual Receipt? Receipt { get; set; }

    public virtual Shiping? Shipping { get; set; }
}
