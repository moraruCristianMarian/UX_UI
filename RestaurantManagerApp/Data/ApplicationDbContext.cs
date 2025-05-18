using RestaurantManagerApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace RestaurantManagerApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<MenuProduct> MenuProducts { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientInProduct> IngredientInProducts { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Review> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension("postgis");



            // ==================================================================
            // RESTAURANT
            // ==================================================================
            // Spacial Reference ID for the geometry
            modelBuilder.Entity<Restaurant>()
                .Property(e => e.Geom)
                .HasColumnType("geometry(Polygon, 4326)");


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
            // PRODUCT
            // ==================================================================
            // Unique name
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();

            // Foreign key to Product Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany(pc => pc.Products)
                .HasForeignKey(p => p.ProductCategoryId);



            // ==================================================================
            // INGREDIENT
            // ==================================================================
            // Unique name
            modelBuilder.Entity<Ingredient>()
                .HasIndex(i => i.Name)
                .IsUnique();



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



            // ==================================================================
            // REVIEW
            // ==================================================================

            // Foreign key to User
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);


            // Polymorphic foreign key to a Product/Restaurant
            modelBuilder.Entity<Review>()
                .HasOne(r => r.ReviewedObjectType)
                .WithMany()
                .HasForeignKey(r => r.ReviewedObjectTypeId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.ReviewedProduct)
                .WithMany(rp => rp.Reviews)
                .HasForeignKey(r => r.ReviewedProductId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.ReviewedRestaurant)
                .WithMany(rr => rr.Reviews)
                .HasForeignKey(r => r.ReviewedRestaurantId);

            //  A review MUST have ONLY an ID to a Restaurant and ReviewedObjectTypeId = 1,
            //                  OR ONLY an ID to a Product    and ReviewedObjectTypeId = 2.
            modelBuilder.Entity<Review>()
                .ToTable(r => r.HasCheckConstraint("CK_Poly_RestaurantOrProduct",
                 "(\"ReviewedObjectTypeId\" = 1 AND \"ReviewedRestaurantId\" IS NOT NULL AND \"ReviewedProductId\" IS NULL) OR" +
                 "(\"ReviewedObjectTypeId\" = 2 AND \"ReviewedRestaurantId\" IS NULL AND \"ReviewedProductId\" IS NOT NULL)"));
        }
    }
}
