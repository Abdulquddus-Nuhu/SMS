using Access.Data.Identity;
using Access.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = string.Empty;
connectionString = Environment.GetEnvironmentVariable("AccessModule_DB_CONNECTION") ?? string.Empty;

//builder.Services.AddDbContext<AccessDbContext>(options =>
//{
//    options.UseInMemoryDatabase("AccessDb");
//});

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
