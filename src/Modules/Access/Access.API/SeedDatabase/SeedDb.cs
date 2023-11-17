using Access.Data;
using Access.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Access.API.SeedDatabase
{
    public class SeedDb : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public SeedDb(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<AccessDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedDb>>();
            try
            {
                logger.LogInformation("Applying Migration!");
                await context.Database.MigrateAsync(cancellationToken: cancellationToken);
                logger.LogInformation("Migration Successful!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to apply Migration!");
            }
            var userManager = scope.ServiceProvider.GetService<UserManager<Persona>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();
            try
            {
                logger.LogInformation("Seeding Data!");
                await SeedIdentity.SeedAsync(userManager, roleManager);
                logger.LogInformation("Seeding Successful!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to execute Data Seeding!");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

}
