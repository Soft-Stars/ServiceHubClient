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
        try
        {
            _logger.LogInformation("Execute");
            var connection = await _signalRClientService.WaitingForYourOrderSir();
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("Receive Cancellation request");

            if (connection != null)
            {
                _logger.LogInformation("Connection is null");
                await connection.StopAsync();
                _logger.LogInformation("Closing connection: {time}", DateTimeOffset.Now);
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex.Message, DateTimeOffset.Now);
        }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker starting at: {time}", DateTimeOffset.Now);
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);
        return base.StopAsync(cancellationToken);
    }
}
