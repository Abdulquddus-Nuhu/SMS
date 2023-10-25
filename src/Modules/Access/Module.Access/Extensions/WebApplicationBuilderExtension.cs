using System;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Module.Access.Data;
using Module.Access.Identity;
using Module.Access.Services.Implementations;
using Module.Access.Services.Interfaces;
using Shared.Constants;
using Shared.Infrastructure.Implementations;
using Shared.Infrastructure.Interfaces;

namespace Module.Access.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static WebApplicationBuilder AddAccessModule(this WebApplicationBuilder builder, IServiceCollection services)
        {
            builder.Services.AddControllers()
                            .AddApplicationPart(typeof(WebApplicationBuilderExtension).Assembly);

            //REgister services
            var connectionString = builder.Configuration.GetConnectionString("MyStarTracker_Db") ?? "Data Source=MyStarTracker_Db.db";
            builder.Services.AddSqlite<AcessDbContext>(connectionString);

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
                     .AddEntityFrameworkStores<AcessDbContext>()
                     .AddDefaultTokenProviders();

            builder.Services.Scan(scan => scan.FromAssemblyOf<IAuthService>()
             .AddClasses(classes => classes.InNamespaceOf<AuthService>())
             .AsImplementedInterfaces()
             .WithTransientLifetime());

            //Shared Services
            builder.Services.Scan(scan => scan.FromAssemblyOf<IOtpGenerator>()
             .AddClasses(classes => classes.InNamespaceOf<OtpGenerator>())
             .AsImplementedInterfaces()
             .WithTransientLifetime());

            //builder.Services.AddSwaggerGen(c =>
            //{
            //    var securityScheme = new OpenApiSecurityScheme
            //    {
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.Http,
            //        Scheme = "bearer",
            //        BearerFormat = "JWT",
            //        In = ParameterLocation.Header,
            //        Description = "JWT Authorization header using the Bearer scheme. **Enter Bearer Token Only**",
            //        Reference = new OpenApiReference
            //        {
            //            Id = "Bearer",
            //            Type = ReferenceType.SecurityScheme
            //        }
            //    };

            //    c.SwaggerDoc("v1", new OpenApiInfo()
            //    {
            //        Version = "v1",
            //        Title = "MyStarTracker API",
            //        Description = "REST API for MyStar Tracker @ Stella Maris Schools",
            //        Contact = new OpenApiContact
            //        {
            //            Name = "Stella Maris Schools",
            //            Email = "dev.nuhu@smsbuja.com",
            //            //Url = new Uri("https://sms.ng")
            //        },
            //    });
            //    c.EnableAnnotations();
            //    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        { securityScheme, Array.Empty<string>() }
            //    });
            //            });

            //Ensure all controllers use jwt token
            builder.Services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                //config.TokenValidationParameters = tokenValidationParams;
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthConstants.Policies.CUSTODIANS, policy => policy.RequireRole(AuthConstants.Roles.ADMIN, AuthConstants.Roles.SUPER_ADMIN));
            });


            return builder;
        }
    }
}

