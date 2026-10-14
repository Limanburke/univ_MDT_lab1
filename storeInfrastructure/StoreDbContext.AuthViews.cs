using Microsoft.EntityFrameworkCore;
using storeDomain.Model;

namespace storeInfrastructure;

public partial class StoreDbContext
{
    public virtual DbSet<VLoginAccount> VLoginAccounts { get; set; }
    public virtual DbSet<VAccountRole> VAccountRoles { get; set; }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VLoginAccount>(entity =>
        {
            entity.HasNoKey().ToView("v_login_accounts");
            entity.Property(e => e.UserAccountId).HasColumnName("user_account_id");
            entity.Property(e => e.Username).HasColumnName("username");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.EmployeeIsDeleted).HasColumnName("employee_is_deleted");
        });

        modelBuilder.Entity<VAccountRole>(entity =>
        {
            entity.HasNoKey().ToView("v_account_roles");
            entity.Property(e => e.UserAccountId).HasColumnName("user_account_id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
        });
    }
}