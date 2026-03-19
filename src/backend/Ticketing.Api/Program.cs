using Ticketing.Api.Extensions;
using Ticketing.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var appConfigConnection = builder.Configuration["AzureResources:AppConfigurationConnectionString"];
if (!string.IsNullOrWhiteSpace(appConfigConnection))
{
    builder.Configuration.AddAzureAppConfiguration(appConfigConnection);
}

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
