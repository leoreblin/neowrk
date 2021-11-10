using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neowrk.Library.Repository;
using System;
using System.Linq;

namespace Neowrk.Library.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            IConfiguration configs = host.Services.GetService<IConfiguration>();

            if (args.Contains("seed"))
            {
                DatabaseBootstrap bookRepository = new DatabaseBootstrap(configs.GetConnectionString("DefaultConnection"));
                bookRepository.Setup().Wait();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
