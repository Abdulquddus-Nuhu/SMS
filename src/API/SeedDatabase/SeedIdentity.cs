using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using static Shared.Constants.AuthConstants;
using static Shared.Constants.StringConstants;

namespace API.SeedDatabase
{
    public class SeedIdentity
    {
        public static async Task SeedAsync(UserManager<Persona>? userManager, RoleManager<Role>? roleManager)
        {
            if (userManager is null || roleManager is null) return;
            await SeedRoles(roleManager);

            //Seed Super Admin
            var superAdminEmail = Environment.GetEnvironmentVariable("ROOT_ADMIN_EMAIL") ?? DefaultValues.ROOT_ADMIN_EMAIL;
            if (await userManager.FindByEmailAsync(superAdminEmail) is null)
            {
                var superAdmin = new Persona()
                {
                    Email = superAdminEmail,
                    FirstName = "Super",
                    LastName = "Admin",
                    PhoneNumber = Environment.GetEnvironmentVariable("ROOT_ADMIN_PHONENUMBER") ?? DefaultValues.ROOT_ADMIN_PHONENUMBER,
                    UserName = superAdminEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };
                var result = await userManager.CreateAsync(superAdmin, Environment.GetEnvironmentVariable("ROOT_DEFAULT_PASS") ?? DefaultValues.ROOT_DEFAULT_PASS);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, Roles.SUPER_ADMIN);
                }
            }
            
            //ADMIN
            var SMSAdmin = Environment.GetEnvironmentVariable("SMS_ADMIN_EMAIL") ?? DefaultValues.SMS_ADMIN_EMAIL;
            if (await userManager.FindByEmailAsync(SMSAdmin) is null)
            {
                var admin = new Persona()
                {
                    Email = SMSAdmin,
                    FirstName = "SMS",
                    LastName = "Admin",
                    PhoneNumber = Environment.GetEnvironmentVariable("SMS_ADMIN_PHONENUMBER") ?? DefaultValues.SMS_ADMIN_PHONENUMBER,
                    UserName = SMSAdmin,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };
                var result = await userManager.CreateAsync(admin, Environment.GetEnvironmentVariable("SMS_ADMIN_PASSWORD") ?? DefaultValues.SMS_ADMIN_PASSWORD);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.ADMIN);
                }
            }

        }
        private static async Task SeedRoles(RoleManager<Role> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Roles.SUPER_ADMIN))
            {
                await roleManager.CreateAsync(new Role(Roles.SUPER_ADMIN));
            }
            if (!await roleManager.RoleExistsAsync(Roles.ADMIN))
            {
                await roleManager.CreateAsync(new Role(Roles.ADMIN));
            }
            if (!await roleManager.RoleExistsAsync(Roles.BUS_DRIVER))
            {
                await roleManager.CreateAsync(new Role(Roles.BUS_DRIVER));
            }
            if (!await roleManager.RoleExistsAsync(Roles.PARENT))
            {
                await roleManager.CreateAsync(new Role(Roles.PARENT));
            }
            if (!await roleManager.RoleExistsAsync(Roles.STUDENT))
            {
                await roleManager.CreateAsync(new Role(Roles.STUDENT));
            }
            if (!await roleManager.RoleExistsAsync(Roles.STAFF))
            {
                await roleManager.CreateAsync(new Role(Roles.STAFF));
            }
        }
    }

}
