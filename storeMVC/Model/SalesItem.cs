using System;
using System.Collections.Generic;

namespace storeDomain.Model;

public partial class SalesItem
{
    public int Id { get; set; }

    public int TransactionId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual SalesTransaction Transaction { get; set; } = null!;
}
