using Microsoft.Extensions.Configuration;
using ServiceHubClient;
using ServiceHubClient.Models;
using ServiceHubClient.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddSingleton<ISignalRClientService, SignalRClientService>();
builder.Services.AddSingleton<ILocalAgentService, LocalAgentService>();
builder.Services.Configure<SignalRSetting>(builder.Configuration.GetSection(nameof(SignalRSetting)));
builder.Services.Configure<LocalAgentSetting>(builder.Configuration.GetSection(nameof(LocalAgentSetting)));
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
