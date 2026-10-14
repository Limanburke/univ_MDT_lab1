using System;
using System.Collections.Generic;

namespace storeDomain.Model;

public partial class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int PositionId { get; set; }

    public DateOnly HireDate { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual Position Position { get; set; } = null!;

    public virtual ICollection<SalesTransaction> SalesTransactions { get; set; } = new List<SalesTransaction>();

    public virtual UserAccount? UpdatedByNavigation { get; set; }

    public virtual UserAccount? UserAccount { get; set; }
}
