using GetTripSystem.Interfaces;
using GetTripSystem.Repositories;
using GetTripSystem.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Configuration;
using System.Data;
using System.Windows;
using static GetTripSystem.DAL;

namespace GetTripSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            services.AddDbContext<Context>(options =>
                options.UseNpgsql("Host=localhost;Database=trip_database;Username=postgres;Password=1234"));

            services.AddScoped<UserRepository>();
            services.AddScoped<TripRepository>();
            services.AddScoped<RegistrationRepository>();
            services.AddScoped<PictureRepository>();

            services.AddScoped<FacadeDB>();

            services.AddScoped<ICreateOperation>(sp => sp.GetRequiredService<FacadeDB>());
            services.AddScoped<IManagement>(sp => sp.GetRequiredService<FacadeDB>());
            services.AddScoped<IRegistration>(sp => sp.GetRequiredService<FacadeDB>());

            services.AddTransient<AuthorizeWindow>();

            ServiceProvider = services.BuildServiceProvider();

            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Context>();
                context.Database.Migrate();
            }

            var authorizeWindow = ServiceProvider.GetRequiredService<AuthorizeWindow>();
            authorizeWindow.Show();
        }
        
    }
}
