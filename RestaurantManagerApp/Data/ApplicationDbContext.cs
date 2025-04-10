using RestaurantManagerApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagerApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<MenuProduct> MenuProducts { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientInProduct> IngredientInProducts { get; set; }
        public DbSet<Image> Images { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // ==================================================================
            // MENU PRODUCT
            // ==================================================================
            // Composite primary key
            modelBuilder.Entity<MenuProduct>()
                .HasKey(mp => new { mp.ProductId, mp.RestaurantId });


            // Foreign keys to Restaurant and Product
            modelBuilder.Entity<MenuProduct>()
                .HasOne(mp => mp.Product)
                .WithMany(p => p.MenuProducts)
                .HasForeignKey(mp => mp.ProductId);

            modelBuilder.Entity<MenuProduct>()
                .HasOne(mp => mp.Restaurant)
                .WithMany(r => r.MenuProducts)
                .HasForeignKey(mp => mp.RestaurantId);



            // ==================================================================
            // INGREDIENT IN PRODUCT
            // ==================================================================
            // Composite primary key
            modelBuilder.Entity<IngredientInProduct>()
                .HasKey(iip => new { iip.IngredientId, iip.ProductId });


            // Foreign keys to Restaurant and Product
            modelBuilder.Entity<IngredientInProduct>()
                .HasOne(iip => iip.Ingredient)
                .WithMany(i => i.IngredientInProducts)
                .HasForeignKey(iip => iip.IngredientId);

            modelBuilder.Entity<IngredientInProduct>()
                .HasOne(iip => iip.Product)
                .WithMany(p => p.IngredientInProducts)
                .HasForeignKey(iip => iip.ProductId);



            // ==================================================================
            // IMAGE
            // ==================================================================
            // Primary key
            modelBuilder.Entity<Image>()
                .HasKey(i => new { i.Id });


            // Foreign key to Restaurant
            modelBuilder.Entity<Image>()
                .HasOne(i => i.Restaurant)
                .WithMany(r => r.Images)
                .HasForeignKey(i => i.RestaurantId);
        }
    }
}
