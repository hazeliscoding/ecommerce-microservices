using BusinessLogicLayer;
using DataAccessLayer;

using FluentValidation.AspNetCore;
using ProductsMicroservice.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessLayer();
builder.Services.AddBusinessLogicLayer();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();