using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.ResourceModels.Admin;
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

        protected override async Task AdditionalValidation(UpsertServiceCommand request)
        {
            var serviceCategory = await Context.ServiceCategories.FindAsync(request.ServiceCategoryId);
            if (serviceCategory.AccountId != CurrentUserManager.CurrentUserAccountId)
                throw new ValidationException(new List<ValidationFailure>()
                {
                    new(nameof(request.ServiceCategoryId), "Service Category does not exist")
                });
        }
    }
}