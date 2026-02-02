using Core.Aplicacion;
using Core.Infraestructura.Extension;
using Core.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Web.PalicacionAPI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options => {
    options.AddPolicy("PermitirTodo", policy => {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AgregarServicioInfraestructura(builder.Configuration);
builder.Services.AgregarServicios();
builder.Services.AddHostedService<CredentialClient>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var redisConfiguration = builder.Configuration.GetSection("Redis")["ConnectionString"];
var redis = ConnectionMultiplexer.Connect(redisConfiguration);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddSingleton<ICacheServicio, CacheServicio>();

WebApplication app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting(); 
app.UseCors("PermitirTodo");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/HealthCheck");

app.Run();