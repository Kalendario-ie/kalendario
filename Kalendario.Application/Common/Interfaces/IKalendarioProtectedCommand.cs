using MediatR;

namespace Kalendario.Application.Common.Interfaces
{
    public interface IKalendarioProtectedCommand<out T> : IRequest<T>
    {
    }
}