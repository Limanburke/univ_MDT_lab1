namespace storeDomain.Model;

public class VLoginAccount
{
    public int UserAccountId { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool IsActive { get; set; }
    public bool EmployeeIsDeleted { get; set; }
}