using ServiceHubClient;
using ServiceHubClient.Models;
using ServiceHubClient.Services;
using NLog.Web;
using NLog.Extensions.Logging;
using NLog;
using Microsoft.Extensions.Hosting.WindowsServices;
using System;
using System.Diagnostics;


var builder = Host.CreateApplicationBuilder(args);

var exePath = AppContext.BaseDirectory;

builder.Configuration
    .SetBasePath(exePath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

//builder.Logging.ClearProviders();
//builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Logging.AddNLog();

builder.Services.AddSingleton<ISignalRClientService, SignalRClientService>();
builder.Services.AddSingleton<ILocalAgentService, LocalAgentService>();
builder.Services.AddSingleton<IPowerShellExecutor, PowerShellExecutor>();
builder.Services.Configure<SignalRSetting>(builder.Configuration.GetSection(nameof(SignalRSetting)));
builder.Services.Configure<LocalAgentSetting>(builder.Configuration.GetSection(nameof(LocalAgentSetting)));

var isWindowsService = !(Debugger.IsAttached || args.Contains("--console"));

if (isWindowsService)
{
    builder.Services.AddSingleton<IHostLifetime, WindowsServiceLifetime>();
}

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

var logger = NLog.LogManager.Setup().GetCurrentClassLogger();  // Initialize NLog logger

try
{
    
    await host.RunAsync();

}
catch (Exception ex)
{
    // Log any critical exceptions during startup
    logger.Error(ex, "Application stopped unexpectedly during setup.");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();  // Ensure NLog resources are flushed and released
}

