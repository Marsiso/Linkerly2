using MediatR;

namespace Linkerly.Domain.Commands;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
