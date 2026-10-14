using System;
using System.Collections.Generic;

namespace storeDomain.Model;

public partial class UserAccount
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<DepartmentStock> DepartmentStocks { get; set; } = new List<DepartmentStock>();

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<UserAccount> InverseUpdatedByNavigation { get; set; } = new List<UserAccount>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<SalesTransaction> SalesTransactions { get; set; } = new List<SalesTransaction>();

    public virtual UserAccount? UpdatedByNavigation { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
