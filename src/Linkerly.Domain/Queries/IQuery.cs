using MediatR;

namespace Linkerly.Domain.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}