using ServiceHubClient.Services;

namespace ServiceHubClient;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ISignalRClientService _signalRClientService;

    public Worker(ILogger<Worker> logger, ISignalRClientService signalRClientService)
    {
        _logger = logger;
        _signalRClientService = signalRClientService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = await _signalRClientService.WaitingForYourOrderSir();
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await Task.Delay(1000, stoppingToken);
        }

        if (connection != null)
        {
            await connection.StopAsync();
            _logger.LogInformation("Closing connection: {time}", DateTimeOffset.Now);
        }
       
    }
}
