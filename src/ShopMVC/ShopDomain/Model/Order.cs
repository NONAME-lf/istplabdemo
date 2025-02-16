namespace ShopDomain.Model;

public partial class Order : Entity
{
    //public int OdId { get; set; }

    public int? OdUser { get; set; }

    public decimal? OdTotal { get; set; }

    public double? OdDiscount { get; set; }

    public string? OdPayment { get; set; }

    public string? OdNotes { get; set; }

    public int? OdReceiptId { get; set; }

    public int? OdProtuctId { get; set; }

    public int? OdShippingId { get; set; }

    public virtual Product? OdProtuct { get; set; }

    public virtual Receipt? OdReceipt { get; set; }

    public virtual Shiping? OdShipping { get; set; }

    public virtual User? OdUserNavigation { get; set; }

    public virtual ICollection<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();
}
