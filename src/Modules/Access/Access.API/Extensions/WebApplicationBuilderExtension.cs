using Access.API.SeedDatabase;
using Access.API.Services.Implementation;
using Access.API.Services.Interfaces;
using Access.Data;
using Access.Data.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Constants;
using Shared.Infrastructure.Implementations;
using Shared.Infrastructure.Interfaces;
using System;
using System.Reflection;
using static Shared.Constants.StringConstants;
using System.Text;
using Access.Core.Interfaces.Repositories;
using Access.Data.Repositories;

namespace Access.API.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static WebApplicationBuilder AddAccessModule(this WebApplicationBuilder builder, IServiceCollection services, IWebHostEnvironment environment)
        {
            builder.Services.AddControllers()
                            .AddApplicationPart(typeof(WebApplicationBuilderExtension).Assembly);

            //Register services
            var connectionString = Environment.GetEnvironmentVariable("AccessModule_DB_CONNECTION") ?? string.Empty;
            builder.Services.AddDbContext<AccessDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            builder.Services.AddIdentity<Persona, Role>(
                     options =>
                     {
                         options.Password.RequireDigit = true;
                         options.Password.RequireNonAlphanumeric = true;
                         options.Password.RequireLowercase = true;
                         options.Password.RequireUppercase = true;
                         options.Password.RequiredLength = 8;
                         options.User.RequireUniqueEmail = true;
                         options.SignIn.RequireConfirmedEmail = true;
                     })
                     .AddEntityFrameworkStores<AccessDbContext>()
                     .AddDefaultTokenProviders();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            builder.Services.AddTransient<IEmailService, SmtpEmailSender>();


            //Repositories
            builder.Services.AddTransient<ICampusRepository, CampusRepository>();
            builder.Services.AddTransient<IGradeRepository, GradeRepository>();
            builder.Services.AddTransient<IJobTitleRepository, JobTitleRepository>();
            builder.Services.AddTransient<IBusRepository, BusRepository>();
            builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();


            //Services
            builder.Services.Scan(scan => scan.FromAssemblyOf<IAuthService>()
             .AddClasses(classes => classes.InNamespaceOf<AuthService>())
             .AsImplementedInterfaces()
             .WithTransientLifetime());
           

            //Shared Services
            builder.Services.Scan(scan => scan.FromAssemblyOf<IOtpGenerator>()
             .AddClasses(classes => classes.InNamespaceOf<OtpGenerator>())
             .AsImplementedInterfaces()
             .WithTransientLifetime());



            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? DefaultValues.JWT_SECRET_KEY);
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = tokenValidationParams;
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthConstants.Policies.ADMINS, policy => policy.RequireRole(AuthConstants.Roles.ADMIN, AuthConstants.Roles.SUPER_ADMIN));
            });

            // Register the worker responsible of seeding the database.
            builder.Services.AddHostedService<SeedDb>();

            return builder;
        }

    }
}
