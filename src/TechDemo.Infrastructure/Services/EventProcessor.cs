
using Microsoft.Extensions.Hosting;

namespace TechDemo.Infrastructure.Services;

public class EventProcessor : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        throw new NotImplementedException();
    }
}