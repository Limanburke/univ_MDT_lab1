using System;

namespace storeDomain.Model;

public class VEmployeeDirectory
{
    public int EmployeeId { get; set; }
    public string FullName { get; set; } = null!;
    public string DepartmentName { get; set; } = null!;
    public string PositionTitle { get; set; } = null!;
    public DateOnly HireDate { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsAccountActive { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedByName { get; set; } = null!;
}