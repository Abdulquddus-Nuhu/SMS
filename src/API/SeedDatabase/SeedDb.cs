using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.SeedDatabase
{
    public class SeedDb : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public SeedDb(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedDb>>();
            try
            {
                logger.LogInformation("Applying MyStar_Db Migration!");
                await context.Database.MigrateAsync(cancellationToken: cancellationToken);
                logger.LogInformation("MyStar_Db Migration Successful!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to apply MyStar_Db Migration!");
            }
            var userManager = scope.ServiceProvider.GetService<UserManager<Persona>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();
            try
            {
                logger.LogInformation("Seeding MyStar_Db Data!");
                await SeedIdentity.SeedAsync(userManager, roleManager);
                logger.LogInformation("Seeding MyStar_Db Successful!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to execute MyStar_Db Data Seeding!");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

}
