using System;
using System.Collections.Generic;

namespace storeDomain.Model;

public partial class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int LoyaltyPoints { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<SalesTransaction> SalesTransactions { get; set; } = new List<SalesTransaction>();
}
