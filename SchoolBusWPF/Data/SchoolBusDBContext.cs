using Microsoft.EntityFrameworkCore;
using SchoolBusWPF.Models.Concretes;

namespace SchoolBusWPF.Data
{
    public class SchoolBusDBContext() : DbContext
    {
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-HMS6GCF\\SQLEXPRESS;Initial Catalog=SchoolBus;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .HasIndex(c => c.PlateNumber)
                .IsUnique();
            modelBuilder.Entity<Driver>()
                .HasIndex(d => d.UserName)
                .IsUnique();
            modelBuilder.Entity<Group>()
                .HasIndex(g => g.Title)
                .IsUnique();
            modelBuilder.Entity<Holiday>()
                .HasIndex(h => h.Name)
                .IsUnique();
            modelBuilder.Entity<Parent>()
                .HasIndex(p => p.UserName)
                .IsUnique();
            modelBuilder.Entity<Driver>()
                .HasOne(d => d.Car)
                .WithOne(c => c.Driver)
                .HasForeignKey<Car>(c => c.DriverId);
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Ride)
                .WithMany(r => r.Students)
                .HasForeignKey(s => s.RideId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
