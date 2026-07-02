using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.Urls.Add("http://localhost:5000");

app.UseCors(options =>
{
    options.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader();
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/purchases", (AppDbContext db) =>
{
    return db.Purchases.ToList();
});

app.MapGet("/purchases/{id}", (int id, AppDbContext db) =>
{
    var purchase = db.Purchases.Find(id);

    if (purchase == null)
        return Results.NotFound();

    return Results.Ok(purchase);
});

app.MapGet("/purchases/search", (string name, AppDbContext db) =>
{
    return db.Purchases
        .Where(p => p.Name == name)
        .ToList();
});

app.MapPost("/purchases", (Purchase purchase, AppDbContext db) =>
{
    purchase.Time = DateTime.Now.ToString("dd.MM.yyyy");

    db.Purchases.Add(purchase);
    db.SaveChanges();

    return Results.Ok(purchase);
});

app.MapDelete("/purchases", (AppDbContext db) =>
{
    var purchases = db.Purchases.ToList();

    db.Purchases.RemoveRange(purchases);
    db.SaveChanges();
    
    return Results.Ok();
});

app.Run();

public class Purchase
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Time {get; set; } 
}

public class AppDbContext : DbContext
{
    public DbSet<Purchase> Purchases => Set<Purchase>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=app.db");
    }
}