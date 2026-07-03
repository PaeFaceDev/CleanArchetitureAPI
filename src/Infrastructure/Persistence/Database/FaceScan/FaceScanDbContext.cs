using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Database.FaceScan
{
    public class FaceScanDbContext : DbContext
    {
        public FaceScanDbContext(DbContextOptions<FaceScanDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => new { e.Workday, e.AccessDate, e.AccessTime });
                entity.ToTable("tbscantime");
                
                entity.Property(e => e.Workday)
                    .HasColumnName("EMPLOYEEID")
                    .HasColumnType("text");
                    
                entity.Property(e => e.AccessDate)
                    .HasColumnName("ACCESSDATE")
                    .HasColumnType("text");
                    
                entity.Property(e => e.AccessTime)
                    .HasColumnName("ACCESSTIME")
                    .HasColumnType("text");
                    
                entity.Property(e => e.FullName)
                    .HasColumnName("PERSONNAME")
                    .HasColumnType("text");
                    
                entity.Property(e => e.Department)
                    .HasColumnName("PERSONGROUP")
                    .HasColumnType("text");
                    
                entity.Property(e => e.EmployeeID)
                    .HasColumnName("HTCPERSONID")
                    .HasColumnType("text");
                    
                entity.Property(e => e.NameTitle)
                    .HasColumnName("TITLE")
                    .HasColumnType("text");
                    
                entity.Property(e => e.Shift)
                    .HasColumnName("SHIFT")
                    .HasColumnType("text");
                    
                entity.Property(e => e.Factory)
                    .HasColumnName("FACTORY")
                    .HasColumnType("text");
            });
        }
    }
}