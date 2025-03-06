using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopDomain.Model;

public partial class Category : Entity
{
    //public int CgId { get; set; }
    
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display (Name = "Категорія")]
    public string? CgName { get; set; }

    [Display (Name = "Під-категорія")]
    public string? CgChildCategory { get; set; }

    [Display (Name = "Опис")]
    public string? CgDescription { get; set; }

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
