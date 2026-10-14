using System;
using System.Collections.Generic;

namespace storeDomain.Model;

public partial class PurchaseOrder
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    public int DepartmentId { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual UserAccount? UpdatedByNavigation { get; set; }
}
