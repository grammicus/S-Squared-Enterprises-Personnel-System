using Microsoft.EntityFrameworkCore;
using PersonnelSystem.Server.Models;

namespace PersonnelSystem.Server.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(e => e.Reports)
                .HasForeignKey(e => e.ManagerId)
                .HasPrincipalKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Manager>().Property(e => e.ManagerId).HasColumnName("ManagerId");

            modelBuilder.Entity<Employee>().Property(e => e.ManagerId).HasColumnName("ManagerId");

            modelBuilder
                .Entity<Role>()
                .HasMany(e => e.Employees)
                .WithMany(e => e.Roles)
                .UsingEntity(
                    l => l.HasOne(typeof(Employee)).WithMany().OnDelete(DeleteBehavior.Restrict),
                    r => r.HasOne(typeof(Role)).WithMany().OnDelete(DeleteBehavior.Restrict)
                );
            //SeedDB
            modelBuilder.Entity<Role>(e =>
            {
                e.HasData(
                    new Role { RoleId = 1, Name = "Director" },
                    new Role { RoleId = 2, Name = "IT" },
                    new Role { RoleId = 3, Name = "Support" },
                    new Role { RoleId = 4, Name = "Accounting" },
                    new Role { RoleId = 5, Name = "Analyst" },
                    new Role { RoleId = 6, Name = "Sales" }
                );
            });
        }
    }
}
