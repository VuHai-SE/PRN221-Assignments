using Microsoft.EntityFrameworkCore;
using SalesDAOs;
using SalesRepositories;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký PRN_Assignment02Context với DI container
builder.Services.AddDbContext<PRN_Assignment02Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionStringDB")));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ProductDAO>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<OrderDAO>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Cấu hình logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
