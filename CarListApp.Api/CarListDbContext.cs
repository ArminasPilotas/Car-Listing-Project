using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarListApp.Api
{
    public class CarListDbContext : IdentityDbContext
    {
        public CarListDbContext(DbContextOptions<CarListDbContext> options) : base(options)
        {
            
        }

        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    Id = 1,
                    Make = "Honda",
                    Model = "Fit",
                    Vin = "ABC"
                },
                new Car
                {
                    Id = 2,
                    Make = "Honda",
                    Model = "Civic",
                    Vin = "ABC"
                },
                new Car
                {
                    Id = 3,
                    Make = "Honda",
                    Model = "Stream",
                    Vin = "ABC"
                }
            );
        }
    }
}
