using MediatR;

namespace Kalendario.Application.Common.Interfaces;

public interface IPublicQuery<out T> : IRequest<T>
{
}