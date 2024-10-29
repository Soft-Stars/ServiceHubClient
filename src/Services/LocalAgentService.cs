using Microsoft.Extensions.Options;
using ServiceHubClient.Models;
using System.Text;

namespace ServiceHubClient.Services
{
    public class LocalAgentService : ILocalAgentService
    {
        private readonly LocalAgentSetting _localAgentSetting;
        private readonly SignalRSetting _signalRSettings;
        private readonly ILogger<LocalAgentService> _logger;
        public LocalAgentService(
            IOptions<LocalAgentSetting> optionsLocalAgentSetting,
            IOptions<SignalRSetting> optionsSignalRSetting,
            ILogger<LocalAgentService> logger)
        {
            _localAgentSetting = optionsLocalAgentSetting.Value;
            _signalRSettings = optionsSignalRSetting.Value;
            _logger = logger;
        }

        public Task Export()
        {
            throw new NotImplementedException();
        }

        public Task Synchronize()
        {
            throw new NotImplementedException();
        }

        public async Task SendMessage(string message)
        {
            var webhookUrl = _localAgentSetting.WebHookUrl;

            var messageToSend = new { text = $"Message from : {_signalRSettings.ClientId}\n {message}" };

            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(messageToSend), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(webhookUrl, content);

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
