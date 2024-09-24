namespace ServiceHubClient.Services
{
    public interface ILocalAgentService
    {
        Task Synchronize();
        Task Export();
    }
}
