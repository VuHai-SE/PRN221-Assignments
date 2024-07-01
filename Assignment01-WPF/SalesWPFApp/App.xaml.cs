using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesBOs;
using SalesBOs.DTOs;
using SalesDAOs;
using SalesRepos;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(context.Configuration, services);
                })
                .Build();
        }

        private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<PRN_AssignmentContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionStringDB")));
            services.AddSingleton<ProductDAO>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<OrderDAO>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<MemberDAO>();
            services.AddSingleton<IMemberRepository, MemberRepository>();
            services.AddSingleton(configuration.GetSection("DefaultAccount").Get<DefaultAccountSettings>());
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginWindow>();
            services.AddTransient<AddProduct>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            var defaultAccountSettings = _host.Services.GetRequiredService<DefaultAccountSettings>();
            InitializeDefaultAccount(defaultAccountSettings);

            //var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            //mainWindow.Show();

            var loginWindow = _host.Services.GetRequiredService<LoginWindow>();
            loginWindow.Show();
            base.OnStartup(e);
        }

        private void InitializeDefaultAccount(DefaultAccountSettings defaultAccountSettings)
        {
            using (var scope = _host.Services.CreateScope())
            {
                var memberRepository = scope.ServiceProvider.GetRequiredService<IMemberRepository>();
                var defaultMember = memberRepository.GetMemberByEmail(defaultAccountSettings.Email);
                if (defaultMember == null)
                {
                    var member = new Member
                    {
                        Email = defaultAccountSettings.Email,
                        Password = defaultAccountSettings.Password,
                        CompanyName = "Default Admin",
                        City = "Admin City",
                        Country = "Admin Country"
                    };
                    memberRepository.Add(member);
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();
            base.OnExit(e);
        }
    }

}
