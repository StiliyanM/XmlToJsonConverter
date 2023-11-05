using Microsoft.AspNetCore.Mvc;
using XmlToJsonConverter.Application.Extensions;
using XmlToJsonConverter.Infrastructure.Extensions;
using XmlToJsonConverter.Web.Extensions;
using XmlToJsonConverter.Web.Middlewares;
using XmlToJsonConverter.Web.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
.AddApplicationServices()
.AddInfrastructureServices(builder.Configuration)
.AddWebServices();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllersWithViews();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();

        var errorResponse = new ErrorDetails(
            StatusCodes.Status400BadRequest, string.Join(", ", errors));

        return new BadRequestObjectResult(errorResponse);
    };
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();