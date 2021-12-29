using MediatR;

namespace Kalendario.Application.Common.Interfaces
{
    public interface IKalendarioQuery<out T> : IRequest<T>
    {
    }
}