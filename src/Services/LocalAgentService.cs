using Microsoft.Extensions.Options;
using ServiceHubClient.Models;

namespace ServiceHubClient.Services
{
    public class LocalAgentService : ILocalAgentService
    {
        private readonly LocalAgentSetting _localAgentSetting;
        private readonly ILogger<LocalAgentService> _logger;
        public LocalAgentService(IOptions<LocalAgentSetting> options, ILogger<LocalAgentService> logger)
        {
            _localAgentSetting = options.Value;
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
    }
}
