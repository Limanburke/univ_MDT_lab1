using System;
using System.Collections.Generic;

namespace storeDomain.Model;

public partial class VActiveProduct
{
    public int? ProductId { get; set; }

    public string? Sku { get; set; }

    public string? ProductName { get; set; }

    public decimal? Price { get; set; }

    public string? CategoryName { get; set; }

    public string? SupplierName { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedByName { get; set; }
}
