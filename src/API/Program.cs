using API.SeedDatabase;
using Core.Interfaces.Infrastructure;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Shared.Constants;
using System.Reflection;
using System.Text;
using static Shared.Constants.StringConstants;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information($"Starting up MyStar Web Server!");

try
{
    var builder = WebApplication.CreateBuilder(args);

    if (builder.Environment.IsProduction())
    {
        builder.WebHost.UseUrls("http://localhost:4001");
    }
    else if (builder.Environment.IsStaging())
    {
        builder.WebHost.UseUrls("http://localhost:4002");
    }

    builder.Logging.ClearProviders();

    if (builder.Environment.IsDevelopment())
    {
        Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           //.WriteTo.File(outputTemplate:"", formatter: "Serilog.Formatting.Json.JsonFormatter, Serilog")
           .CreateLogger();
    }
    else if (builder.Environment.IsStaging())
    {
        Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Information()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
           .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File(
                @"/logs/MyStarAppStaging/logs.txt",
                fileSizeLimitBytes: 10485760,
                rollOnFileSizeLimit: true,
                shared: true,
                retainedFileCountLimit: null,
                flushToDiskInterval: TimeSpan.FromSeconds(1))
           //.WriteTo.ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces)
           .CreateLogger();
    }
    else
    {
        Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Information()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
           .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File(
                @"/logs/MyStarAppProduction/logs.txt",
                fileSizeLimitBytes: 10485760,
                rollOnFileSizeLimit: true,
                shared: true,
                retainedFileCountLimit: null,
                flushToDiskInterval: TimeSpan.FromSeconds(1))
           //.WriteTo.ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces)
           .CreateLogger();
    }

    builder.Host.UseSerilog();
    builder.Services.AddControllers();



    //Register services
    var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? string.Empty;
    builder.Services.AddDbContext<AppDbContext>(options =>
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
             .AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders();

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    builder.Services.AddTransient<IEmailService, SmtpEmailSender>();


    //Repositories
    builder.Services.AddTransient<IBusRepository, BusRepository>();
    builder.Services.AddTransient<ICampusRepository, CampusRepository>();
    builder.Services.AddTransient<IGradeRepository, GradeRepository>();
    builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
    builder.Services.AddTransient<IJobTitleRepository, JobTitleRepository>();
    builder.Services.AddTransient<IQrCodeRepository, QrCodeRepository>();
    builder.Services.AddTransient<ITripRepository, TripRepository>();
    builder.Services.AddTransient<IStudentRepository, StudentRepository>();
    builder.Services.AddTransient<IBusDriverRepository, BusDriverRepository>();
    builder.Services.AddTransient<IParentRepository, ParentRepository>();


    //Services
    builder.Services.AddTransient<IAuthService, AuthService>();
    builder.Services.AddTransient<IPersonaService, PersonaService>();
    builder.Services.AddTransient<ITokenService, TokenService>();
    builder.Services.AddTransient<IBusService, BusService>();
    builder.Services.AddTransient<ICampusService, CampusService>();
    builder.Services.AddTransient<IGradeService, GradeService>();
    builder.Services.AddTransient<IDepartmentService, DepartmentService>();
    builder.Services.AddTransient<IJobTitleService, JobTitleService>();
    builder.Services.AddTransient<IQrCodeService, QrCodeService>();
    builder.Services.AddTransient<ITripService, TripService>();



    builder.Services.AddTransient<IOtpGenerator, OtpGenerator>();


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


    //Ensure all controllers use jwt token
    builder.Services.AddControllers(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    });

 

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(d =>
    {
        d.SwaggerDoc("API-Host", new OpenApiInfo()
        {
            Version = "v1",
            Title = "MyStarAPI",
            Description = "REST API for MyStar App - Stella Maris Schools",
            Contact = new OpenApiContact
            {
                Name = "Stella Maris Schools",
                Email = "dev.nuhu@smsbuja.com",
                //Url = new Uri("https://sms.ng")
            },
        });

        d.SwaggerDoc("SPE Module", new OpenApiInfo
        {
            Title = "SPE Module",
            Version = "v1",
            Description = "SPE Module APIs",
            Contact = new OpenApiContact
            {
                Name = "Stella Maris Schools",
                Email = "dev.nuhu@smsbuja.com",
                //Url = new Uri("https://sms.ng")
            },
        });
    });

    //Swagger Authentication/Authorization
    builder.Services.AddSwaggerGen(c =>
    {
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. **Enter Bearer Token Only**",
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        };

        c.EnableAnnotations();
        c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { securityScheme, Array.Empty<string>() }
        });
    });

    //Add Cors
    const string CORS_POLICY = "CorsPolicy";
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(CORS_POLICY,
                          builder =>
                          {
                              builder.WithOrigins(new string[]
                              {
                                  "http://www.mystarsonline.com",
                                  "https://www.mystarsonline.com"
                              });
                              //builder.AllowAnyOrigin();
                              builder.AllowAnyMethod();
                              builder.AllowAnyHeader();
                          });
    });

    // Security and Production enhancements 
    if (!builder.Environment.IsDevelopment())
    {
        // Proxy Server Config
        builder.Services.Configure<ForwardedHeadersOptions>(
              options =>
              {
                  options.ForwardedHeaders =
                      ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
              });

        //Persist key
        builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("/var/keys"));
    }

    //Remove Server Header
    builder.WebHost.UseKestrel(options => options.AddServerHeader = false);


    // Create the directory if it doesn't exist
    var path = Path.Combine(builder.Environment.ContentRootPath, "static");
    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }


    var app = builder.Build();

    // Configure the HTTP request pipeline.

    if (!app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            //c.SwaggerEndpoint($"/swagger/API-Host/swagger.json", "API-Host");
            c.SwaggerEndpoint($"/swagger/SPE Module/swagger.json", "SPE Module");

            //const string swaggerRoutePrefix = "api-docs";
            //c.RoutePrefix = swaggerRoutePrefix;
            //foreach (var description in provider.ApiVersionDescriptions)
            //{
            //    c.SwaggerEndpoint(
            //        $"/swagger/{description.GroupName}/swagger.json",
            //        description.GroupName.ToUpperInvariant());
            //}
        });

    }

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "static")),
        RequestPath = "/static"
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during bootstrapping the Server!");
}
finally
{
    Log.CloseAndFlush();
}


public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    readonly IApiVersionDescriptionProvider _provider;
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
        this._provider = provider;
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo()
                {
                    Version = description.ApiVersion.ToString(),
                    Title = "KNHRMS Services API",
                    Description = "Web API for KNHMRS Services",
                    TermsOfService = new Uri("https://www.swdohcskano.com.ng/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Hubuk Technology LTD",
                        Email = "info@hubuk.ng",
                        Url = new Uri("https://hubuk.ng")
                    },
                });
        }
    }
}
