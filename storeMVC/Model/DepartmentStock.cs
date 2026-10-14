using System;
using System.Collections.Generic;

namespace storeDomain.Model;

public partial class DepartmentStock
{
    public int DepartmentId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual UserAccount? UpdatedByNavigation { get; set; }
}
