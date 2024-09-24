namespace ServiceHubClient.Models
{
    public class SignalRSetting
    {
        public string Url { get; set; }
        public string AccessToken { get; set; }
        public string ClientId { get; set; }
        public int DelayBeforeReconnect { get; set; }
    }
}
