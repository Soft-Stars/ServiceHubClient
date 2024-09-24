using Microsoft.AspNetCore.SignalR.Client;

namespace ServiceHubClient.Services
{
    public interface ISignalRClientService
    {
        Task<HubConnection?> WaitingForYourOrderSir();
    }
}
