using FluentValidation;
using MediatR;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResult
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validator is null) return await next();

        var result = await _validator.ValidateAsync(request, cancellationToken);

        if (result.IsValid) return await next();

        var errors = result.Errors.ConvertAll(error => new ValidationError(
            error.PropertyName,
            error.ErrorMessage));

        var validationError = new Error(
            "Validation error.",
            "One or more validation error ocurred.",
            errors);

        return (dynamic)validationError;
    }
}