using ShopInfrastructure;
using Microsoft.EntityFrameworkCore;
using ShopDomain;
using ShopDomain.Model;
using ShopInfrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ShopDbContext>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddScoped<IDataPortServiceFactory<Category>, CategoryDataPortServiceFactory>();

builder.Services.AddDistributedMemoryCache(); // Необхідно для сесій
builder.Services.AddSession(); // Додаємо сесії

var app = builder.Build();

app.UseSession();

app.MapControllerRoute(
    name: "products_create",
    pattern: "Products/Create/{categoryId?}",
    defaults: new { controller = "Products", action = "Create" });


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Categories}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();