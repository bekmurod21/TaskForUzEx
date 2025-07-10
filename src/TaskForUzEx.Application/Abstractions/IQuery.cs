using MediatR;

namespace TaskForUzEx.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}