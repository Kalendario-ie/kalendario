using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Commands.Admin.Common;

public abstract class BaseDeleteAdminCommand : IKalendarioProtectedCommand<Unit>
{
    [JsonIgnore] public Guid Id { get; set; }

    public abstract class BaseDeleteAdminCommandHandler<TRequest, TDomain> :
        IRequestHandler<TRequest>
        where TRequest : BaseDeleteAdminCommand
        where TDomain : AccountEntity, new()
    {
        protected IKalendarioDbContext Context;
        protected readonly ICurrentUserManager CurrentUserManager;

        public BaseDeleteAdminCommandHandler(IKalendarioDbContext context, ICurrentUserManager currentUserManager)
        {
            Context = context;
            CurrentUserManager = currentUserManager;
        }

        public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var domain = await Context.Set<TDomain>()
                    .Where(e => e.AccountId == CurrentUserManager.CurrentUserAccountId)
                    .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (domain == default)
            {
                throw new NotFoundException(nameof(TDomain), request.Id);
            }

            Context.Set<TDomain>().Remove(domain);

            await ExtraDeletes(request, cancellationToken);
            
            await Context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        protected abstract Task ExtraDeletes(TRequest request, CancellationToken cancellationToken);
    }
}