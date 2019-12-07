using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventDispatcher.EntityFrameworkCore.Samples.InMemory
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            await Run(host);
        }

        private static async Task Run(IHost host)
        {
            using (IServiceScope scope = host.Services.CreateScope())
            {
                SampleDbContext dbContext = scope.ServiceProvider.GetService<SampleDbContext>();
                User user = new User
                {
                    Name = "John Smith"
                };
                dbContext.Users.Add(user);
                user.SayHello();
                user.SayMerryChristmas();
                user.SayThereIsNoSpoon();
                Console.WriteLine("Saving User.");
                await dbContext.SaveChangesAsync();
                Console.WriteLine("Saving User Completed.");
                Console.ReadKey();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddEntityFrameworkInMemoryDatabase();
                    services.AddEventDispatcher();
                    services.AddEventHandler<HelloWorldDomainEvent, HelloWorldDomainEventHandler>();
                    services.AddEventHandler<HelloWorldDomainEvent, HelloWorldDomainEventHandler2>();
                    services.AddEventHandler<HelloWorldIntegrationEvent, HelloWorldIntegrationEventHandler>();
                    services.AddEventHandler<MerryChristmasDomainEvent, MerryChristmasDomainEventHandler>();
                    services.AddEventHandler<MerryChristmasIntegrationEvent, MerryChristmasIntegrationEventHandler>();
                    services.AddEventHandler<ThereIsNoSpoonEvent, ThereIsNoSpoonEventHandler>();
                    services.AddDbContext<SampleDbContext>((serviceProvider, options) =>
                    {
                        options.UseInMemoryDatabase("Sample");
                        options.UseInternalServiceProvider(serviceProvider);
                    });
                });
        }
    }
}
