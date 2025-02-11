using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Kfz.Database;
using Microsoft.EntityFrameworkCore;

namespace Kfz
{
    class Program
    {

        static void Main(string[] args)
        {
            var config1 = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            using (IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<KfzDbContext>(options => {
                        options.UseSqlServer(config1.GetConnectionString("Kfz")); 
                    });
                })
                .Build()
            )
            {
                KfzDbContext? kfzDbContext = host.Services.GetService<KfzDbContext>();

                if(kfzDbContext == null)
                {
                    return;
                }

                KfzService kfzService = new(kfzDbContext);

                KfzConfigurator kfzConfigurator = new(kfzService, new ConsoleHelper());

                Console.WriteLine("Willkommen bei der Kfz-Konfigurator-App.");
                Console.WriteLine("Wenn Sie eine Operation abbrechen möchten, tippen Sie bitte 'q' und drücken Sie Enter.");
                Console.WriteLine();

                kfzConfigurator.Run();
            }
        }
    }
}
