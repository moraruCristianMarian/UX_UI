using RestaurantManagerApp.Data;
using RestaurantManagerApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = "Host=localhost;Port=5432;Database=UxUiDb;Username=postgres;Password=postgres";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "restaurants",
    pattern: "restaurants",
    defaults: new { controller = "Restaurants", action = "Index" });

app.MapControllerRoute(
    name: "products",
    pattern: "products",
    defaults: new { controller = "Products", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();



//  CREATE
app.MapPost("/restaurant", async (ApplicationDbContext db, Restaurant r) =>
{
    db.Restaurants.Add(r);
    await db.SaveChangesAsync();

    return Results.Created($"/restaurant/{r.Id}", r);
});
//  CREATE
app.MapPost("/product", async (ApplicationDbContext db, Product p) =>
{
    db.Products.Add(p);
    await db.SaveChangesAsync();

    return Results.Created($"/product/{p.Id}", p);
});
//  CREATE
app.MapPost("/ingredient", async (ApplicationDbContext db, Ingredient i) =>
{
    db.Ingredients.Add(i);
    await db.SaveChangesAsync();

    return Results.Created($"/ingredient/{i.Id}", i);
});
//  CREATE
app.MapPost("/menuproduct", async (ApplicationDbContext db, MenuProduct mp) =>
{
    db.MenuProducts.Add(mp);
    await db.SaveChangesAsync();

    return Results.Created($"/menuproduct/{mp.ProductId}_{mp.RestaurantId}", mp);
});
//  CREATE
app.MapPost("/ingredientinproduct", async (ApplicationDbContext db, IngredientInProduct iip) =>
{
    db.IngredientInProducts.Add(iip);
    await db.SaveChangesAsync();

    return Results.Created($"/ingredientinproduct/{iip.IngredientId}_{iip.ProductId}", iip);
});


//  READ ALL
app.MapGet("/restaurant", async (ApplicationDbContext db) =>
{
    return await db.Restaurants.ToListAsync();
});

//  READ ONE
app.MapGet("/restaurant/{id}", (ApplicationDbContext db, Guid id) =>
{
    return db.Restaurants.Find(id);
});

//  UPDATE
app.MapPut("/restaurant/{id}", async (ApplicationDbContext db, Restaurant r, Guid id) =>
{
    var updatedRestaurant = db.Restaurants.Find(id);
    if (updatedRestaurant == null)
        return;

    updatedRestaurant.Name = r.Name;
    updatedRestaurant.City = r.City;

    await db.SaveChangesAsync();
});

//  DELETE
app.MapDelete("/restaurant/{id}", async (ApplicationDbContext db, Guid id) =>
{
    var deletedRestaurant = db.Restaurants.Find(id);
    if (deletedRestaurant == null)
        return;

    db.Restaurants.Remove(deletedRestaurant);
    await db.SaveChangesAsync();
});


app.Run();
