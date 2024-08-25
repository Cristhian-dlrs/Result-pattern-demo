using MediatR;
using Microsoft.Extensions.Logging;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResult
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Received Request {@RequestName}, {@Date}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

        var result = await next();

        if (result.IsFailure)
        {
            _logger.LogInformation(
                "Fail to process Request {@RequestName}, {@Error}, {@Date}",
                typeof(TRequest).Name,
                result.Error is null ? result.Errors : result.Error,
                DateTime.UtcNow);

        }

        _logger.LogInformation(
            "Processed Request {@RequestName}, {@Date}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

        return result;
    }
}