using MediatR;

namespace Kalendario.Application.Common.Interfaces;

public interface IKalendarioCommand<out T> : IRequest<T>
{
    
}