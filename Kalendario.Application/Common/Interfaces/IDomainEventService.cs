using System.Threading.Tasks;
using Kalendario.Core.Common;

namespace Kalendario.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}