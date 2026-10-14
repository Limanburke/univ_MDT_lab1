using System;
using System.Collections.Generic;

namespace storeDomain.Model;

public partial class Product
{
    public int Id { get; set; }

    public string Sku { get; set; } = null!;

    public int CategoryId { get; set; }

    public int SupplierId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<DepartmentStock> DepartmentStocks { get; set; } = new List<DepartmentStock>();

    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

    public virtual ICollection<SalesItem> SalesItems { get; set; } = new List<SalesItem>();

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual UserAccount? UpdatedByNavigation { get; set; }

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}
