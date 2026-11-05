using Lab02Models.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lab02Models.Services;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
    : base(options)

    {
    }


    public DbSet<Department> Department => Set<Department>();
    
    public DbSet<Employee> Employee => Set<Employee>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Locker> Lockers => Set<Locker>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Assignment> Assignments => Set<Assignment>();


    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Building> Buildings => Set<Building>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)

    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Course>().HasKey(c => new { c.Code, c.Number });
        modelBuilder.Entity<Assignment>().HasKey(a => new { a.EmployeeId, a.ProjectId });

        // Seed fixed primary keys and link related records through foreign keys.
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Information Technology", Budget = 120000m });
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, FirstName = "Alex", LastName = "Smith", DepartmentId = 1 },
            new Employee { Id = 2, FirstName = "Jordan", LastName = "Lee", DepartmentId = 1 });
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, name = "Electronics" });
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, name = "Keyboard", Price = 49.99m, CategoryId = 1 },
            new Product { Id = 2, name = "Mouse", Price = 24.99m, CategoryId = 1 });
        modelBuilder.Entity<Project>().HasData(
            new Project { Id = 1, Name = "Website Redesign" },
            new Project { Id = 2, Name = "Inventory System" });
        modelBuilder.Entity<Assignment>().HasData(
            new Assignment { EmployeeId = 1, ProjectId = 1, Role = "Developer", Hours = 20m },
            new Assignment { EmployeeId = 1, ProjectId = 2, Role = "Tester", Hours = 10m },
            new Assignment { EmployeeId = 2, ProjectId = 1, Role = "Designer", Hours = 15m });
        modelBuilder.Entity<Locker>().HasData(
            new Locker { Id = 1, Number = "L101", EmployeeId = 1 });
        modelBuilder.Entity<Course>().HasData(
            new Course { Code = "CSCI", Number = "3110", CreditHours = 3 },
            new Course { Code = "CSCI", Number = "1250", CreditHours = 4 });
        modelBuilder.Entity<Building>().HasData(
            new Building { BuildingNumber = 100, Name = "Science Hall" },
            new Building { BuildingNumber = 200, Name = "Technology Center" });

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Locker)
            .WithOne(locker => locker.Employee)
            .HasForeignKey<Locker>(locker => locker.EmployeeId);
    }
}