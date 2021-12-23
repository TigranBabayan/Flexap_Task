
using Flexap_Task.Models;
using Microsoft.EntityFrameworkCore;

namespace Flexap_Task.Data
{
    public class ApplicationContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGalery> Galeries { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Order> Orders { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();  
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductGalery>()
                .HasOne(p => p.Product)
                .WithMany(t => t.ProductGaleries)
                .HasForeignKey(p => p.ProductId);

           
        }
    }
}
