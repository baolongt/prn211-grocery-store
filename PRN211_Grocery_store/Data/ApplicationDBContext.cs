using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PRN211_Grocery_store.Data.Entity;
using System.IO;

namespace PRN211_Grocery_store.Data
{
    public class ApplicationDBContext : DbContext
    {
        public string ConnectionString { get; set; }
        public ApplicationDBContext()
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration config = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", true, true)
                                        .Build();
                ConnectionString = config["ConnectionString:PRN211"];
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Category>().ToTable("Category");
        }
    }
}
