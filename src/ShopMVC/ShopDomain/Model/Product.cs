using ShopInfrastructure;

namespace ShopDomain.Model;

public partial class Product : Entity
{
    //public int PdId { get; set; }

    public decimal? PdPrice { get; set; }

    public string? PdMeasurements { get; set; }

    public int? PdQuantity { get; set; }

    public double? PdDiscount { get; set; }

    public string? PdAbout { get; set; }

    public int? PdManufacturerId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Manufacturer? PdManufacturer { get; set; }

    public virtual ICollection<ProductCart> ProductCarts { get; set; } = new List<ProductCart>();

    public virtual ICollection<ProductCatergory> ProductCatergories { get; set; } = new List<ProductCatergory>();

    public virtual ICollection<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();
}
