using MediatR;

namespace Kalendario.Application.Common.Interfaces;

public interface IKalendarioProtectedRequest<out T> : IRequest<T>
{
    
}