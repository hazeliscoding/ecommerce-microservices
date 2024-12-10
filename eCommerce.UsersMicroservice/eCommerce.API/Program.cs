using eCommerce.Core;
using eCommerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddInfrastructure();
builder.Services.AddCore();

// Add controllers to the container
builder.Services.AddControllers();

// Build the web application
var app = builder.Build();

// Add middleware for exception handling
app.UseExceptionHandler();

// Routing
app.UseRouting();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Controller routes
app.MapControllers();
app.Run();
