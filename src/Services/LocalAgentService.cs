using Microsoft.Extensions.Options;
using ServiceHubClient.Models;
using System.Text;

namespace ServiceHubClient.Services
{
    public class LocalAgentService : ILocalAgentService
    {
        private readonly LocalAgentSetting _localAgentSetting;
        private readonly SignalRSetting _signalRSettings;
        private readonly IPowerShellExecutor _powerShellExecutor;
        private readonly ILogger<LocalAgentService> _logger;

        public LocalAgentService(
            IPowerShellExecutor powerShellExecutor,
            IOptions<LocalAgentSetting> optionsLocalAgentSetting,
            IOptions<SignalRSetting> optionsSignalRSetting,
            ILogger<LocalAgentService> logger)
        {
            _localAgentSetting = optionsLocalAgentSetting.Value;
            _signalRSettings = optionsSignalRSetting.Value;
            _logger = logger;
            _powerShellExecutor = powerShellExecutor;
        }

        public Task Export()
        {
            throw new NotImplementedException();
        }

        public Task Synchronize()
        {
            throw new NotImplementedException();
        }

        public async Task ExecuteScript(string script)
        {
            _logger.LogInformation("ExecuteScript...");

            var report = _powerShellExecutor.ExecuteScript(script, "");

            await LogAction("ExecuteScript", report);
        }

        public async Task SendMessage(string message)
        {
            _logger.LogInformation("SendMessage...");
            var webhookUrl = _localAgentSetting.WebHookUrl;

            var messageToSend = new { text = $"Message from : {_signalRSettings.ClientId}\n {message}" };

            HttpResponseMessage response;

            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(messageToSend), Encoding.UTF8, "application/json");

                response = await client.PostAsync(webhookUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Message sent successfully!");
                }
                else
                {
                    _logger.LogInformation($"Error sending message: {response.StatusCode}");
                }
            }

            await LogAction("SendMessage", response.IsSuccessStatusCode ? "mesage sent successfully" : "Error while sending message");
        }

        private async Task LogAction(string action, string comment)
        {
            using (var client = new HttpClient())
            {
                var messageToSend = new { clientId = _signalRSettings.ClientId, action = action, comment = comment };
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(messageToSend), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_signalRSettings.LogUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Message sent successfully!");
                }
                else
                {
                    _logger.LogInformation($"Error sending message: {response.StatusCode}");
                }
            }
        }
    }
}
