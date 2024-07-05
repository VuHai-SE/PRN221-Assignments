using Microsoft.EntityFrameworkCore;
using SalesDAOs;
using SalesRazorPageApp.Pages.Orders;
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
builder.Services.AddScoped<OrderDetailDAO>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<MemberDAO>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddLogging();

// Cấu hình logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

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
