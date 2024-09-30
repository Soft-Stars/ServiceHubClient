using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using ServiceHubClient.Models;

namespace ServiceHubClient.Services
{
    public class SignalRClientService : ISignalRClientService
    {
        private readonly SignalRSetting _signalRSetting;
        private readonly ILogger<SignalRClientService> _logger;
        private readonly ILocalAgentService _localAgentService;

        public SignalRClientService(
            IOptions<SignalRSetting> options,
            ILogger<SignalRClientService> logger,
            ILocalAgentService localAgentService)
        {
            _signalRSetting = options.Value;
            _logger = logger;
            _localAgentService = localAgentService;
        }
        public async Task<HubConnection?> WaitingForYourOrderSir()
        {
            HubConnection? connection = null;

            while (connection == null || connection?.State == HubConnectionState.Disconnected)
            {
                var hubUrl = _signalRSetting.Url;

                // Create a connection to the SignalR hub
                connection = new HubConnectionBuilder()
                    .WithUrl(hubUrl, options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(_signalRSetting.AccessToken);
                        // options.Headers.Add("Authorization", "Bearer your-access-token");
                        options.Headers.Add("CI", _signalRSetting.ClientId);
                        options.Headers.Add("T", DateTime.UtcNow.ToString("o"));
                    })
                    .WithAutomaticReconnect()
                    .Build();

                // Define a handler for when the connection is closed
                connection.Closed += async (error) =>
                {
                    bool connectionSucceded = false;
                    while (connectionSucceded == false)
                    {
                        try
                        {
                            _logger.LogInformation("Connection closed. Trying to reconnect...");
                            await Task.Delay(_signalRSetting.DelayBeforeReconnect);
                            await connection.StartAsync();
                            _logger.LogInformation("RecConnected to the SignalR hub.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation($"Connection error: {ex.Message}");
                        }
                    }
                };

                // Handle any messages from the server (this matches the method in the hub)
                connection.On<string>("Execute", async (message) =>
                {
                    _logger.LogInformation($"Received message: {message}");

                    if (message == "synchronize")
                    {
                        await _localAgentService.Synchronize();
                    }

                    if (message == "export")
                    {
                        await _localAgentService.Export();
                    }
                });

                try
                {
                    // Start the connection to the SignalR hub
                    await connection.StartAsync();
                    _logger.LogInformation("Connected to the SignalR hub.");

                    _logger.LogInformation("Waiting for message...");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Connection error: {ex.Message}");
                }

                if (connection == null || connection?.State == HubConnectionState.Disconnected)
                {
                    await Task.Delay(_signalRSetting.DelayBeforeReconnect);
                    _logger.LogInformation("Connection not started Trying to reconnect...");
                }
            }

            return connection;
        }
    }
}
