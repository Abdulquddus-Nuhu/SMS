using Access.API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;


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

    //Ensure all controllers use jwt token
    builder.Services.AddControllers(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    });

    //Register Modules
    builder.AddAccessModule(builder.Services, builder.Environment);

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
                              builder.WithOrigins();
                              //builder.WithOrigins(new string[]
                              //{
                              //    "http://localhost:3000",
                              //    "https://localhost:3000",
                              //});
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
            c.SwaggerEndpoint($"/swagger/API-Host/swagger.json", "API-Host");
            c.SwaggerEndpoint($"/swagger/SPE Module/swagger.json", "SPE Module");
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

