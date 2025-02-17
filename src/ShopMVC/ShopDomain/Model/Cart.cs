namespace ShopDomain.Model;

public partial class Cart : Entity
{
    //public new int Id { get; set; }

    public int? CtQuantity { get; set; }

    public int? CtPrice { get; set; }

    public int? CtUserId { get; set; }

    public virtual User? CtUser { get; set; }

    public virtual ICollection<ProductCart> ProductCarts { get; set; } = new List<ProductCart>();
}
