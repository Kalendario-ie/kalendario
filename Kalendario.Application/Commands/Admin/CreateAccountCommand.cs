using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Authorization;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Core.Entities;
using MediatR;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Account), Account.CreateRole)]
public class CreateAccountCommand : IKalendarioProtectedCommand<Guid>
{
    public string Name { get; set; }

    public class Handler : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly IKalendarioDbContext _context;
        private readonly ICurrentUserManager _currentUserManager;

        public Handler(IKalendarioDbContext context, ICurrentUserManager currentUserManager)
        {
            _context = context;
            _currentUserManager = currentUserManager;
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

            await _currentUserManager.AddToRoleAsync(AuthorizationHelper.MasterRole);

            await _currentUserManager
                .RemoveFromRoleAsync(AuthorizationHelper.RoleName(typeof(Account), Account.CreateRole));

            await _currentUserManager.AddToAccountAsync(account.Id);

            return account.Id;
        }
    }
}