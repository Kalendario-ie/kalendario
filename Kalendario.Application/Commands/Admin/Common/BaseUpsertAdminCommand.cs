using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Authorization;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Commands.Admin.Common;

public abstract class BaseUpsertAdminCommand<TResult> : IKalendarioProtectedCommand<TResult>
{
    [JsonIgnore] public Guid? Id { get; set; }

    public abstract class BaseUpsertAdminCommandHandler<TRequest, TDomain, TResourceModel> :
        IRequestHandler<TRequest, TResourceModel>
        where TRequest : BaseUpsertAdminCommand<TResourceModel>
        where TDomain : AccountEntity, new()
    {
        protected readonly IKalendarioDbContext Context;
        protected readonly IMapper Mapper;
        protected readonly ICurrentUserManager CurrentUserManager;

        public BaseUpsertAdminCommandHandler(IKalendarioDbContext context, IMapper mapper,
            ICurrentUserManager currentUserManager)
        {
            Context = context;
            Mapper = mapper;
            CurrentUserManager = currentUserManager;
        }

        public async Task<TResourceModel> Handle(TRequest request, CancellationToken cancellationToken)
        {
            if (!(await CurrentUserManager.IsInRoleAsync(AuthorizationHelper.RoleName(typeof(TDomain),
                    BaseEntity.CreateRole))) && !request.Id.HasValue)
            {
                throw new ForbiddenAccessException();
            }

            if (!(await CurrentUserManager.IsInRoleAsync(AuthorizationHelper.RoleName(typeof(TDomain),
                    BaseEntity.UpdateRole))) && request.Id.HasValue)
            {
                throw new ForbiddenAccessException();
            }

            await AdditionalValidation(request);

            var domain = await (request.Id.HasValue
                ? Entities
                    .Where(e => e.AccountId == CurrentUserManager.CurrentUserAccountId)
                    .FirstOrDefaultAsync(e => e.Id == request.Id.Value, cancellationToken)
                : Task.FromResult(new TDomain()));

            if (domain == default)
            {
                throw new NotFoundException(nameof(TDomain), request.Id);
            }

            if (domain.Id == Guid.Empty)
            {
                Context.Set<TDomain>().Add(domain);
                domain.AccountId = CurrentUserManager.CurrentUserAccountId;
            }

            UpdateDomain(domain, request);

            await Context.SaveChangesAsync(cancellationToken);

            return Mapper.Map<TResourceModel>(domain);
        }

        protected abstract IQueryable<TDomain> Entities { get; }

        protected abstract void UpdateDomain(TDomain domain, TRequest request);
        protected abstract Task AdditionalValidation(TRequest request);
    }
}