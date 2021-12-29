using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Domain;
using MediatR;

namespace Kalendario.Application.Commands.Admin;

public class CreateAccountRequest : IKalendarioCommand<Unit>
{
    public string Name { get; set; }

    public class Handler : IRequestHandler<CreateAccountRequest>
    {
        private readonly IKalendarioDbContext _context;

        public Handler(IKalendarioDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
        {
            var account = new Account
            {
                Name = request.Name
            };

            await _context.Accounts.AddAsync(account, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}