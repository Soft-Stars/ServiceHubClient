namespace ServiceHubClient.Services
{
    public interface ILocalAgentService
    {
        Task Synchronize();
        Task Export();
        Task SendMessage(string message);

        Task ExecuteScript(string script);
    }
}
