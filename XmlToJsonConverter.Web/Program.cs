using XmlToJsonConverter.Application.Extensions;
using XmlToJsonConverter.Infrastructure.Extensions;
using XmlToJsonConverter.Web.Extensions;
using XmlToJsonConverter.Web.Middlewares;

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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