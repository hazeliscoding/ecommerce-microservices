using eCommerce.API.Middlewares;
using eCommerce.Core;
using eCommerce.Infrastructure;
using System.Text.Json.Serialization;
using eCommerce.Core.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddInfrastructure();
builder.Services.AddCore();

// Add controllers to the container
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(ApplicationUserMappingProfile).Assembly);

// Build the web application
var app = builder.Build();

// Add middleware for exception handling
app.UseExceptionHandlingMiddleware();

// Routing
app.UseRouting();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Controller routes
app.MapControllers();
app.Run();
