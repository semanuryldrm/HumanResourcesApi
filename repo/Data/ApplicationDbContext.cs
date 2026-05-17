using HumanResourcesApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HumanResourcesApi.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Payroll> Payrolls { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Department>()
            .HasMany(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Employee>()
            .HasMany(e => e.Payrolls)
            .WithOne(p => p.Employee)
            .HasForeignKey(p => p.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Department>()
            .Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Entity<Employee>()
            .Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Entity<Employee>()
            .Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Entity<Employee>()
            .Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Entity<Employee>()
            .Property(e => e.GrossSalary)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Payroll>()
            .Property(p => p.GrossSalary)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Payroll>()
            .Property(p => p.DeductionAmount)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Payroll>()
            .Property(p => p.NetSalary)
            .HasColumnType("decimal(18,2)");
    }
}