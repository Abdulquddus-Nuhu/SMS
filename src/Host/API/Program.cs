using Microsoft.OpenApi.Models;
//using Module.Access;
//using Module.Access.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Register Modules
//builder.AddAccessModule(builder.Services);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(d =>
{
    d.SwaggerDoc("API-Host", new OpenApiInfo()
    {
        Version = "v1",
        Title = "MyStarApp API",
        Description = "REST API for MyStar App @ Stella Maris Schools",
        Contact = new OpenApiContact
        {
            Name = "Stella Maris Schools",
            Email = "dev.nuhu@smsbuja.com",
        },
    });

    d.SwaggerDoc("Access", new OpenApiInfo
    {
        Title = "Access Module",
        Version = "v1",
        Description = "Access Module",
        Contact = new OpenApiContact
        {
            Name = "itsfinniii"
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/API-Host/swagger.json", "API-Host");
        c.SwaggerEndpoint("/swagger/Access/swagger.json", "Access");
    });
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/API-Host/swagger.json", "API-Host");
    c.SwaggerEndpoint("/swagger/Access/swagger.json", "Access");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

