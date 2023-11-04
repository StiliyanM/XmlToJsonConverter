using XmlToJsonConverter.Application.Extensions;
using XmlToJsonConverter.Infrastructure.Extensions;
using XmlToJsonConverter.Web.Extensions;
using XmlToJsonConverter.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services
.AddApplicationServices()
.AddInfrastructureServices(builder.Configuration)
.AddWebServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandlingMiddleware>();

