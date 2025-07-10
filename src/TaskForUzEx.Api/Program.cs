using Serilog;
using TaskForUzEx.Api.Extensions;
using TaskForUzEx.Api.Extensions.Configurations;
using TaskForUzEx.Api.Middlewares;
using TaskForUzEx.Api.WebSockets;
using TaskForUzEx.Application;
using TaskForUzEx.Infrastructure;
using TaskForUzEx.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddSingleton<WebSocketHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomServices();
builder.ConfigureCORSPolicy();
builder.Services.AddHttpContextAccessor();

var logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .Enrich
    .FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Host.UseSerilog(logger);

var app = builder.Build();
app.ApplyMigration();
app.InitAccessor();
if (app.Environment.IsDevelopment()|| app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "TaskForUzEx");
        options.ConfigObject.AdditionalItems["persistAuthorization"] = "true";
    });
}
app.UseCors("AllowAll");
app.UseCors("AllowHeaders");



app.UseWebSockets();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<WebSocketMiddleware>();
app.MapControllers();

app.Run();