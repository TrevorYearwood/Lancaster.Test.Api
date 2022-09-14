using Markerstudy.Lancaster.Api.Middleware;
using Markerstudy.Lancaster.Application;
using Markerstudy.Lancaster.Infrastructure;
using Markerstudy.Lancaster.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = Directory.GetCurrentDirectory()
});

builder.WebHost.UseIISIntegration();

Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration));

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateAsyncScope())
{
	try
	{
        var dbContext = scope.ServiceProvider.GetRequiredService<LancasterDbContext>();
        await dbContext.Database.MigrateAsync();
    }   
	catch (Exception ex)
	{
        Log.Fatal(ex.Message, ex);
        throw new Exception("An error occurred whilst migrating or seeding the database.", ex);
    }
}

app.UseDeveloperExceptionPage();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
