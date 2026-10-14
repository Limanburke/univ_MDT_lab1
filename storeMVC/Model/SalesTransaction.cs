using System;
using System.Collections.Generic;

namespace storeDomain.Model;

public partial class SalesTransaction
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public int EmployeeId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<SalesItem> SalesItems { get; set; } = new List<SalesItem>();

    public virtual UserAccount? UpdatedByNavigation { get; set; }
}
