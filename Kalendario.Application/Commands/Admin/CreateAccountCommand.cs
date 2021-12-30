using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Core.Domain;
using MediatR;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Account), Account.CreateRole)]
public class CreateAccountCommand : IKalendarioProtectedCommand<Guid>
{
    public string Name { get; set; }

    public class Handler : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly IKalendarioDbContext _context;

        public Handler(IKalendarioDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
        {
            if (_context.Accounts.FirstOrDefault(a => a.Name == command.Name) != default)
            {
                throw new ValidationException()
                {
                    Errors = {[nameof(command.Name)] = new[] {"Name is already being used."}}
                };
            }

            var account = new Account
            {
                Name = command.Name
            };

            await _context.Accounts.AddAsync(account, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return account.Id;
        }
    }
}