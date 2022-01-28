using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Validators;
using Kalendario.Core.Entities;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Service), $"{Service.CreateRole},{Service.UpdateRole}")]
public class UpsertServiceCommand : BaseUpsertAdminCommand<ServiceAdminResourceModel>
{
    public string Name { get; set; }

    public string Description { get; set; }

    public double Price { get; set; }

    public TimeSpan Duration { get; set; }

    public Guid? ServiceCategoryId { get; set; }

    public class Handler : BaseUpsertAdminCommandHandler<UpsertServiceCommand, Service, ServiceAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserManager currentUserManager)
            : base(context, mapper, currentUserManager)
        {
        }

        protected override IQueryable<Service> Entities => Context.Services;

        protected override void UpdateDomain(Service domain, UpsertServiceCommand request)
        {
            domain.Name = request.Name;
            domain.Description = request.Description;
            domain.Price = request.Price;
            domain.Duration = request.Duration;
            domain.ServiceCategoryId = request.ServiceCategoryId;
        }

        protected override async Task AdditionalValidation(UpsertServiceCommand request,
            CancellationToken cancellationToken)
        {
            if (request.ServiceCategoryId.HasValue)
                await ValidationUtils.ThrowIfNotExist<ServiceCategory>(request.ServiceCategoryId.Value, Context, CurrentUserManager);
        }
    }
}