using DMLab3CRRUDM2M.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMLab3CRRUDM2M.Services;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed IDs start at 101 (doctors) and 201 (patients) so they are unlikely to clash with
        // rows already in the database, and so URLs clearly show which ID is which.
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { Id = 101, Name = "Dr. Amara Okafor", Specialty = "Cardiology" },
            new Doctor { Id = 102, Name = "Dr. Benjamin Hayes", Specialty = "Pediatrics" },
            new Doctor { Id = 103, Name = "Dr. Carmen Lopez", Specialty = "Dermatology" },
            new Doctor { Id = 104, Name = "Dr. David Chen", Specialty = "Family Medicine" },
            new Doctor { Id = 105, Name = "Dr. Elena Petrova", Specialty = "Neurology" });

        modelBuilder.Entity<Patient>().HasData(
            new Patient { Id = 201, Name = "Frank Miller" },
            new Patient { Id = 202, Name = "Grace Kim" },
            new Patient { Id = 203, Name = "Hector Ramirez" },
            new Patient { Id = 204, Name = "Isabel Turner" },
            new Patient { Id = 205, Name = "Jamal Wright" });
    }
}
