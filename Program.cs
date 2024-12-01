using Atelier_2.Context;
using Atelier_2.Models.Repositories;
using Atelier_2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register AppDbContext for Product (only once)
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductDBConnection"))
);

// Register repositories
builder.Services.AddScoped<IRepository<Product>, SqlProductRepository>();

// Add ASP.NET Core Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()  // Use your AppDbContext for Identity
    .AddDefaultTokenProviders();  // Add default token providers for things like password reset

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication and Authorization middleware
app.UseAuthentication();  // Ensure authentication is set up
app.UseAuthorization();   // Ensure authorization is set up

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
