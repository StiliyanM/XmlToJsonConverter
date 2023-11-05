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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();

        var errorResponse = new ErrorDetails(StatusCodes.Status400BadRequest, string.Join(", ", errors));

        return new BadRequestObjectResult(errorResponse);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();