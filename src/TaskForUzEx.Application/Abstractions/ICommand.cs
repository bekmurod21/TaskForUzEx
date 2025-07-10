using MediatR;

namespace TaskForUzEx.Application.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}