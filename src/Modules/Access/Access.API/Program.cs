using Access.Data.Identity;
using Access.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Access.API.Services.Implementation;
using Access.API.Services.Interfaces;
using Shared.Infrastructure.Implementations;
using Shared.Infrastructure.Interfaces;
using System.Reflection;
using Access.Core.Interfaces.Repositories;
using Access.Data.Repositories;
using static Shared.Constants.StringConstants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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


var connectionString = Environment.GetEnvironmentVariable("AccessModule_DB_CONNECTION") ?? DefaultValues.AccessModule_DB_CONNECTION;
builder.Services.AddDbContext<AccessDbContext>(options =>
{
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Access.Data"));
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
    options.AddPolicy(AuthConstants.Policies.ADMINS, policy =>
    {
        policy.RequireRole(AuthConstants.Roles.ADMIN, AuthConstants.Roles.SUPER_ADMIN);
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});


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




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();
