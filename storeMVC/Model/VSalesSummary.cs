using System;

namespace storeDomain.Model;

public partial class VSalesSummary
{
    public int? TransactionId { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? Status { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? DepartmentName { get; set; }
    public string? CashierName { get; set; }
    public string? CustomerName { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedByName { get; set; }
}