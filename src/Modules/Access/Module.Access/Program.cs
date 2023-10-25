using System;
using Microsoft.AspNetCore.Identity;
using Module.Access.Data;
using Module.Access.Identity;
using Module.Access.Services.Implementations;
using Module.Access.Services.Interfaces;
using Shared.Infrastructure.Implementations;
using Shared.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

