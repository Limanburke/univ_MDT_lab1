using System;
using System.Collections.Generic;

namespace storeDomain.Model;

public partial class VDepartmentInventory
{
    public int? DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public int? ProductId { get; set; }

    public string? Sku { get; set; }

    public string? ProductName { get; set; }

    public int? Quantity { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedByName { get; set; }
}
