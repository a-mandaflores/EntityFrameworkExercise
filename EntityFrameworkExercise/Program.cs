using EntityFrameworkExercise.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
var services = builder.Services;


// Serviços MVC, etc.
services.AddControllers();

var connectionString = configuration.GetConnectionString("StoreContext")
    ?? throw new InvalidOperationException("Connection string 'StoreContext' not found.");

services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlite(connectionString, p =>
    {
        p.MigrationsHistoryTable("__EFMigrationsHistory", "store");
    });
});

services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.CustomSchemaIds(type => type.FullName);
});
services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
    try
    {
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

await app.RunAsync();
