using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShopInfrastructure;

public partial class ShopDbContext : DbContext
{
    public ShopDbContext()
    {
    }

    public ShopDbContext(DbContextOptions<ShopDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCart> ProductCarts { get; set; }

    public virtual DbSet<ProductCatergory> ProductCatergories { get; set; }

    public virtual DbSet<ProductOrder> ProductOrders { get; set; }

    public virtual DbSet<Receipt> Receipts { get; set; }

    public virtual DbSet<Shiping> Shipings { get; set; }

    public virtual DbSet<ShippingCompany> ShippingCompanies { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=master;User Id=SA;Password=Rfybreks1;TrustServerCertificate=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("carts");

            entity.HasIndex(e => e.CtUserId, "unique_user_cart").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ct_id");
            entity.Property(e => e.CtPrice).HasColumnName("ct_price");
            entity.Property(e => e.CtQuantity).HasColumnName("ct_quantity");
            entity.Property(e => e.CtUserId).HasColumnName("ct_user_id");

            entity.HasOne(d => d.CtUser).WithOne(p => p.Cart)
                .HasForeignKey<Cart>(d => d.CtUserId)
                .HasConstraintName("FK_carts_users");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("categories");

            entity.Property(e => e.Id).HasColumnName("cg_id");
            entity.Property(e => e.CgChildCategory)
                .HasMaxLength(100)
                .HasColumnName("cg_child_category");
            entity.Property(e => e.CgDescription)
                .HasMaxLength(1000)
                .HasColumnName("cg_description");
            entity.Property(e => e.CgName)
                .HasMaxLength(100)
                .HasColumnName("cg_name");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("countries");

            entity.Property(e => e.Id).HasColumnName("co_id");
            entity.Property(e => e.CoName)
                .HasMaxLength(50)
                .HasColumnName("co_name");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("manufacturers");

            entity.Property(e => e.Id).HasColumnName("mn_id");
            entity.Property(e => e.MnContactInfo)
                .HasMaxLength(1000)
                .HasColumnName("mn_contact_info");
            entity.Property(e => e.MnCountry).HasColumnName("mn_country");
            entity.Property(e => e.MnName)
                .HasMaxLength(50)
                .HasColumnName("mn_name");

            entity.HasOne(d => d.MnCountryNavigation).WithMany(p => p.Manufacturers)
                .HasForeignKey(d => d.MnCountry)
                .HasConstraintName("FK_manufacturers_countries");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("od_id");
            entity.Property(e => e.OdDiscount).HasColumnName("od_discount");
            entity.Property(e => e.OdNotes)
                .HasMaxLength(100)
                .HasColumnName("od_notes");
            entity.Property(e => e.OdPayment)
                .HasMaxLength(50)
                .HasColumnName("od_payment");
            entity.Property(e => e.OdProtuctId).HasColumnName("od_protuct_id");
            entity.Property(e => e.OdReceiptId).HasColumnName("od_receipt_id");
            entity.Property(e => e.OdShippingId).HasColumnName("od_shipping_id");
            entity.Property(e => e.OdTotal)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("od_total");
            entity.Property(e => e.OdUser).HasColumnName("od_user");

            entity.HasOne(d => d.OdProtuct).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OdProtuctId)
                .HasConstraintName("FK_orders_products");

            entity.HasOne(d => d.OdReceipt).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OdReceiptId)
                .HasConstraintName("FK_orders_receipts");

            entity.HasOne(d => d.OdShipping).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OdShippingId)
                .HasConstraintName("FK_orders_shipings");

            entity.HasOne(d => d.OdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OdUser)
                .HasConstraintName("FK_orders_users");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_product");

            entity.ToTable("products");

            entity.Property(e => e.Id).HasColumnName("pd_id");
            entity.Property(e => e.PdAbout)
                .HasMaxLength(1000)
                .HasColumnName("pd_about");
            entity.Property(e => e.PdDiscount).HasColumnName("pd_discount");
            entity.Property(e => e.PdManufacturerId).HasColumnName("pd_manufacturer_id");
            entity.Property(e => e.PdMeasurements)
                .HasMaxLength(10)
                .HasColumnName("pd_measurements");
            entity.Property(e => e.PdPrice)
                .HasColumnType("money")
                .HasColumnName("pd_price");
            entity.Property(e => e.PdQuantity).HasColumnName("pd_quantity");

            entity.HasOne(d => d.PdManufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.PdManufacturerId)
                .HasConstraintName("FK_products_manufacturers");
        });

        modelBuilder.Entity<ProductCart>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("product_carts");

            entity.Property(e => e.Id).HasColumnName("pc_id");
            entity.Property(e => e.PcCart).HasColumnName("pc_cart");
            entity.Property(e => e.PcPrice)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("pc_price");
            entity.Property(e => e.PcProduct).HasColumnName("pc_product");
            entity.Property(e => e.PcQuantity).HasColumnName("pc_quantity");

            entity.HasOne(d => d.PcCartNavigation).WithMany(p => p.ProductCarts)
                .HasForeignKey(d => d.PcCart)
                .HasConstraintName("FK_product_carts_carts");

            entity.HasOne(d => d.PcProductNavigation).WithMany(p => p.ProductCarts)
                .HasForeignKey(d => d.PcProduct)
                .HasConstraintName("FK_product_carts_products");
        });

        modelBuilder.Entity<ProductCatergory>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("product_catergories");

            entity.Property(e => e.Id).HasColumnName("pct_id");
            entity.Property(e => e.PctCategoryId).HasColumnName("pct_category_id");
            entity.Property(e => e.PctProductId).HasColumnName("pct_product_id");

            entity.HasOne(d => d.PctCategory).WithMany(p => p.ProductCatergories)
                .HasForeignKey(d => d.PctCategoryId)
                .HasConstraintName("FK_product_catergories_categories_1");

            entity.HasOne(d => d.PctProduct).WithMany(p => p.ProductCatergories)
                .HasForeignKey(d => d.PctProductId)
                .HasConstraintName("FK_product_catergories_products");
        });

        modelBuilder.Entity<ProductOrder>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("product_orders");

            entity.Property(e => e.Id).HasColumnName("po_id");
            entity.Property(e => e.PoOrderId).HasColumnName("po_order_id");
            entity.Property(e => e.PoPrice)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("po_price");
            entity.Property(e => e.PoProductId).HasColumnName("po_product_id");
            entity.Property(e => e.PoQuantity).HasColumnName("po_quantity");

            entity.HasOne(d => d.PoOrder).WithMany(p => p.ProductOrders)
                .HasForeignKey(d => d.PoOrderId)
                .HasConstraintName("FK_product_orders_orders_1");

            entity.HasOne(d => d.PoProduct).WithMany(p => p.ProductOrders)
                .HasForeignKey(d => d.PoProductId)
                .HasConstraintName("FK_product_orders_products");
        });

        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("receipts");

            entity.Property(e => e.Id).HasColumnName("rp_id");
            entity.Property(e => e.RpAbout)
                .HasMaxLength(100)
                .HasColumnName("rp_about");
            entity.Property(e => e.RpDateCreated)
                .HasColumnType("datetime")
                .HasColumnName("rp_date_created");
            entity.Property(e => e.RpDiscount)
                .HasMaxLength(10)
                .HasColumnName("rp_discount");
            entity.Property(e => e.RpPayment)
                .HasMaxLength(10)
                .HasColumnName("rp_payment");
            entity.Property(e => e.RpQuantity).HasColumnName("rp_quantity");
            entity.Property(e => e.RpShippingId).HasColumnName("rp_shipping_id");
            entity.Property(e => e.RpTotal)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("rp_total");

            entity.HasOne(d => d.RpShipping).WithMany(p => p.Receipts)
                .HasForeignKey(d => d.RpShippingId)
                .HasConstraintName("FK_receipts_shipings");
        });

        modelBuilder.Entity<Shiping>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("shipings");

            entity.Property(e => e.Id).HasColumnName("sh_id");
            entity.Property(e => e.ShAdress)
                .HasMaxLength(100)
                .HasColumnName("sh_adress");
            entity.Property(e => e.ShCountryId).HasColumnName("sh_country_id");
            entity.Property(e => e.ShShippingCompanyId).HasColumnName("sh_shipping_company_id");

            entity.HasOne(d => d.ShCountry).WithMany(p => p.Shipings)
                .HasForeignKey(d => d.ShCountryId)
                .HasConstraintName("FK_shipings_countries");

            entity.HasOne(d => d.ShShippingCompany).WithMany(p => p.Shipings)
                .HasForeignKey(d => d.ShShippingCompanyId)
                .HasConstraintName("FK_shipings_shipping_companies");
        });

        modelBuilder.Entity<ShippingCompany>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("shipping_companies");

            entity.Property(e => e.Id).HasColumnName("sc_id");
            entity.Property(e => e.ScAvgTimeNeed)
                .HasMaxLength(100)
                .HasColumnName("sc_avg_time_need");
            entity.Property(e => e.ScName).HasColumnName("sc_name");
            entity.Property(e => e.ScPricing)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("sc_pricing");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_users");

            entity.ToTable("// users");

            entity.Property(e => e.Id).HasColumnName("ur_id");
            entity.Property(e => e.UrBirthdate)
                .HasColumnType("datetime")
                .HasColumnName("ur_birthdate");
            entity.Property(e => e.UrCountryId).HasColumnName("ur_country_id");
            entity.Property(e => e.UrEmail)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ur_email");
            entity.Property(e => e.UrNickname)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ur_nickname");
            entity.Property(e => e.UrRole)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ur_role");

            entity.HasOne(d => d.UrCountry).WithMany(p => p.Users)
                .HasForeignKey(d => d.UrCountryId)
                .HasConstraintName("FK_users_countries");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
