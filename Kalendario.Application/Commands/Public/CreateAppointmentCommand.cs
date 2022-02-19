using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Results.Public;
using MediatR;

namespace Kalendario.Application.Commands.Public;

public class CreateAppointmentCommand : IKalendarioProtectedCommand<CreateAppointmentResult>
{
    public class Handler : IRequestHandler<CreateAppointmentCommand, CreateAppointmentResult>
    {
        public Handler(IKalendarioDbContext kalendarioDbContext)
        {
            
        }
        
        public Task<CreateAppointmentResult> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}