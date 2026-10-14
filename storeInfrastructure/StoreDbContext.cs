using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace storeInfrastructure;

using storeDomain.Model;

public partial class StoreDbContext : DbContext
{
    public StoreDbContext() { }

    public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DepartmentStock> DepartmentStocks { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SalesItem> SalesItems { get; set; }

    public virtual DbSet<SalesTransaction> SalesTransactions { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<VActiveProduct> VActiveProducts { get; set; }

    public virtual DbSet<VDepartmentInventory> VDepartmentInventories { get; set; }

    public virtual DbSet<VUserAuditLog> VUserAuditLogs { get; set; }

    public virtual DbSet<VSalesSummary> VSalesSummaries { get; set; }

    public virtual DbSet<VActiveDepartment> VActiveDepartments { get; set; }

    public virtual DbSet<VActiveEmployee> VActiveEmployees { get; set; }

    public virtual DbSet<VActiveCustomer> VActiveCustomers { get; set; }

    public virtual DbSet<VEmployeeDirectory> VEmployeeDirectories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pkey");
            entity.ToTable("category");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.ParentCategoryId).HasColumnName("parent_category_id");
            entity.HasOne(d => d.ParentCategory)
                .WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("category_parent_category_id_fkey");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_pkey");
            entity.ToTable("customer");
            entity.HasIndex(e => e.Phone, "customer_phone_key").IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasMaxLength(50).HasColumnName("first_name");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.LastName).HasMaxLength(50).HasColumnName("last_name");
            entity.Property(e => e.LoyaltyPoints).HasColumnName("loyalty_points");
            entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("phone");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("department_pkey");
            entity.ToTable("department");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.HasOne(d => d.Manager)
                .WithMany(p => p.Departments)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("fk_department_manager");
        });

        modelBuilder.Entity<DepartmentStock>(entity =>
        {
            entity.HasKey(e => new { e.DepartmentId, e.ProductId }).HasName("pk_department_stock");
            entity.ToTable("department_stock");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.HasOne(d => d.Department)
                .WithMany(p => p.DepartmentStocks)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("department_stock_department_id_fkey");
            entity.HasOne(d => d.Product)
                .WithMany(p => p.DepartmentStocks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("department_stock_product_id_fkey");
            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany(p => p.DepartmentStocks)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("department_stock_updated_by_fkey");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_pkey");
            entity.ToTable("employee");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.FirstName).HasMaxLength(50).HasColumnName("first_name");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.LastName).HasMaxLength(50).HasColumnName("last_name");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.HasOne(d => d.Department)
                .WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_department_id_fkey");
            entity.HasOne(d => d.Position)
                .WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_position_id_fkey");
            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany(p => p.Employees)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_employee_updated_by");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("position_pkey");
            entity.ToTable("position");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BaseSalary).HasPrecision(12, 2).HasColumnName("base_salary");
            entity.Property(e => e.Title).HasMaxLength(100).HasColumnName("title");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");
            entity.ToTable("product");
            entity.HasIndex(e => e.Name, "idx_product_active_name").HasFilter("(is_deleted = false)");
            entity.HasIndex(e => e.Sku, "product_sku_key").IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name).HasMaxLength(200).HasColumnName("name");
            entity.Property(e => e.Price).HasPrecision(12, 2).HasColumnName("price");
            entity.Property(e => e.Sku).HasMaxLength(50).HasColumnName("sku");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_category_id_fkey");
            entity.HasOne(d => d.Supplier)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_supplier_id_fkey");
            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("product_updated_by_fkey");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("promotion_pkey");
            entity.ToTable("promotion");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiscountPercentage).HasPrecision(5, 2).HasColumnName("discount_percentage");
            entity.Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_date");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_date");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany(p => p.Promotions)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("promotion_updated_by_fkey");
            entity.HasMany(d => d.Products)
                .WithMany(p => p.Promotions)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductPromotion",
                    r => r.HasOne<Product>()
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("product_promotion_product_id_fkey"),
                    l => l.HasOne<Promotion>()
                        .WithMany()
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("product_promotion_promotion_id_fkey"),
                    j =>
                    {
                        j.HasKey("PromotionId", "ProductId").HasName("pk_product_promotion");
                        j.ToTable("product_promotion");
                        j.IndexerProperty<int>("PromotionId").HasColumnName("promotion_id");
                        j.IndexerProperty<int>("ProductId").HasColumnName("product_id");
                    });
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("purchase_order_pkey");
            entity.ToTable("purchase_order");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.OrderDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("order_date");
            entity.Property(e => e.Status).HasMaxLength(50).HasColumnName("status");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.HasOne(d => d.Department)
                .WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("purchase_order_department_id_fkey");
            entity.HasOne(d => d.Supplier)
                .WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("purchase_order_supplier_id_fkey");
            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("purchase_order_updated_by_fkey");
        });

        modelBuilder.Entity<PurchaseOrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("purchase_order_item_pkey");
            entity.ToTable("purchase_order_item");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExpectedPrice).HasPrecision(12, 2).HasColumnName("expected_price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.PurchaseOrderId).HasColumnName("purchase_order_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.HasOne(d => d.Product)
                .WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("purchase_order_item_product_id_fkey");
            entity.HasOne(d => d.PurchaseOrder)
                .WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("purchase_order_item_purchase_order_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");
            entity.ToTable("role");
            entity.HasIndex(e => e.Name, "role_name_key").IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<SalesItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sales_item_pkey");
            entity.ToTable("sales_item");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(12, 2)
                .HasColumnName("unit_price");
            entity.HasOne(d => d.Product)
                .WithMany(p => p.SalesItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sales_item_product_id_fkey");
            entity.HasOne(d => d.Transaction)
                .WithMany(p => p.SalesItems)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sales_item_transaction_id_fkey");
        });

        modelBuilder.Entity<SalesTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sales_transaction_pkey");
            entity.ToTable("sales_transaction");
            entity.HasIndex(e => e.TransactionDate, "idx_sales_transaction_date");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Completed'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(12, 2)
                .HasColumnName("total_amount");
            entity.Property(e => e.TransactionDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("transaction_date");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.HasOne(d => d.Customer)
                .WithMany(p => p.SalesTransactions)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("sales_transaction_customer_id_fkey");
            entity.HasOne(d => d.Department)
                .WithMany(p => p.SalesTransactions)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sales_transaction_department_id_fkey");
            entity.HasOne(d => d.Employee)
                .WithMany(p => p.SalesTransactions)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sales_transaction_employee_id_fkey");
            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany(p => p.SalesTransactions)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("sales_transaction_updated_by_fkey");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("supplier_pkey");
            entity.ToTable("supplier");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContactInfo)
                .HasMaxLength(255)
                .HasColumnName("contact_info");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_account_pkey");
            entity.ToTable("user_account");
            entity.HasIndex(e => e.EmployeeId, "user_account_employee_id_key").IsUnique();
            entity.HasIndex(e => e.Username, "user_account_username_key").IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("LOCALTIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
            entity.HasOne(d => d.Employee)
                .WithOne(p => p.UserAccount)
                .HasForeignKey<UserAccount>(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_account_employee_id_fkey");
            entity.HasOne(d => d.UpdatedByNavigation)
                .WithMany(p => p.InverseUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("user_account_updated_by_fkey");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserAccountId, e.RoleId }).HasName("pk_user_role");
            entity.ToTable("user_role");
            entity.Property(e => e.UserAccountId).HasColumnName("user_account_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.AssignedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("assigned_at");
            entity.HasOne(d => d.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_role_role_id_fkey");
            entity.HasOne(d => d.UserAccount)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_role_user_account_id_fkey");
        });

        modelBuilder.Entity<VActiveProduct>(entity =>
        {
            entity.HasNoKey()
                .ToView("v_active_products");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
            entity.Property(e => e.Price)
                .HasPrecision(12, 2)
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(200)
                .HasColumnName("product_name");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .HasColumnName("sku");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(150)
                .HasColumnName("supplier_name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedByName).HasColumnName("updated_by_name");
        });

        modelBuilder.Entity<VDepartmentInventory>(entity =>
        {
            entity.HasNoKey()
                .ToView("v_department_inventory");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .HasColumnName("department_name");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(200)
                .HasColumnName("product_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .HasColumnName("sku");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedByName).HasColumnName("updated_by_name");
        });

        modelBuilder.Entity<VUserAuditLog>(entity =>
        {
            entity.HasNoKey()
                .ToView("v_user_audit_log");
            entity.Property(e => e.EntityId)
                .HasColumnType("character varying")
                .HasColumnName("entity_id");
            entity.Property(e => e.EntityType)
                .HasColumnName("entity_type");
            entity.Property(e => e.EntityDetails)
                .HasColumnName("entity_details");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<VSalesSummary>(entity =>
        {
            entity.HasNoKey()
                .ToView("v_sales_summary");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.TransactionDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("transaction_date");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(12, 2)
                .HasColumnName("total_amount");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .HasColumnName("department_name");
            entity.Property(e => e.CashierName).HasColumnName("cashier_name");
            entity.Property(e => e.CustomerName).HasColumnName("customer_name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedByName).HasColumnName("updated_by_name");
        });

        modelBuilder.Entity<VActiveDepartment>(entity =>
        {
            entity.HasNoKey()
            .ToView("v_active_departments");
            entity.Property(e => e.Id)
            .HasColumnName("id");
            entity.Property(e => e.Name)
            .HasColumnName("name");
        });

        modelBuilder.Entity<VActiveEmployee>(entity =>
        {
            entity.HasNoKey()
            .ToView("v_active_employees");
            entity.Property(e => e.Id)
            .HasColumnName("id");
            entity.Property(e => e.FullName)
            .HasColumnName("full_name");
            entity.Property(e => e.DepartmentId)
            .HasColumnName("department_id");
        });

        modelBuilder.Entity<VActiveCustomer>(entity =>
        {
            entity.HasNoKey()
            .ToView("v_active_customers");
            entity.Property(e => e.Id)
            .HasColumnName("id");
            entity.Property(e => e.FullName)
            .HasColumnName("full_name");
            entity.Property(e => e.Phone)
            .HasColumnName("phone");
        });

        modelBuilder.Entity<VEmployeeDirectory>(entity =>
        {
            entity.HasNoKey()
            .ToView("v_employee_directory");
            entity.Property(e => e.EmployeeId)
            .HasColumnName("employee_id");
            entity.Property(e => e.FullName)
            .HasColumnName("full_name");
            entity.Property(e => e.DepartmentName)
            .HasColumnName("department_name");
            entity.Property(e => e.PositionTitle)
            .HasColumnName("position_title");
            entity.Property(e => e.HireDate)
            .HasColumnName("hire_date");
            entity.Property(e => e.IsDeleted)
            .HasColumnName("is_deleted");
            entity.Property(e => e.IsAccountActive)
            .HasColumnName("is_account_active");
            entity.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedByName).HasColumnName("updated_by_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}