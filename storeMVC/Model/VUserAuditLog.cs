namespace storeDomain.Model;

public partial class VUserAuditLog
{
    public string? Username { get; set; }

    public string? EntityType { get; set; }

    public string? EntityId { get; set; }

    public string? EntityDetails { get; set; }

    public DateTime? UpdatedAt { get; set; }
}