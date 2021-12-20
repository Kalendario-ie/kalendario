using MediatR;

namespace Kalendario.Application.Common.Interfaces
{
    public interface IQuery<out T> : IRequest<T>
    {
    }
}