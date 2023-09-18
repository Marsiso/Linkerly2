using FluentValidation;
using Linkerly.Domain.Exceptions;
using Linkerly.Domain.Validations;
using MediatR;

namespace Linkerly.Application.Validations;

public class FluentValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public FluentValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        ArgumentNullException.ThrowIfNull(validators);

        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();

        var validationContext = new ValidationContext<TRequest>(request);

        var validationResults = _validators.Select(validator => validator.Validate(validationContext));

        var validationFailures = validationResults.DistinctErrorsByProperty();

        if (validationFailures.Any()) throw new EntityValidationException(validationFailures);

        return await next();
    }
}