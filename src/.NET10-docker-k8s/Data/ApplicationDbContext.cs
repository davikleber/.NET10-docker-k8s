using Microsoft.EntityFrameworkCore;
using Net10.docker.k8s.Model;

namespace Net10.docker.k8s.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; } = null!;
        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FirstName).HasMaxLength(100);
                entity.Property(p => p.LastName).HasMaxLength(100);
                entity.Property(p => p.Address).HasMaxLength(250);
                entity.Property(p => p.Gender).HasMaxLength(50);
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Car");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Make).HasMaxLength(100);
                entity.Property(c => c.Model).HasMaxLength(100);
                entity.Property(c => c.Color).HasMaxLength(50);
                entity.Property(c => c.Vin).HasMaxLength(50);
            });
        }
    }
}
