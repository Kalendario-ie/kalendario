using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;
using Kalendario.Core.ValueObject;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(ServiceCategory), $"{ServiceCategory.CreateRole},{ServiceCategory.UpdateRole}")]
public class UpsertServiceCategoryCommand : BaseUpsertAdminCommand<ServiceCategoryAdminResourceModel>
{
    public string Name { get; set; }

    public string Colour { get; set; }


    public class Handler : BaseUpsertAdminCommandHandler<UpsertServiceCategoryCommand, ServiceCategory,
        ServiceCategoryAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserManager currentUserManager)
            : base(context, mapper, currentUserManager)
        {
        }

        protected override IQueryable<ServiceCategory> Entities => Context.ServiceCategories;

        protected override void UpdateDomain(ServiceCategory domain, UpsertServiceCategoryCommand request)
        {
            domain.Name = request.Name;
            domain.Colour = Core.ValueObject.Colour.From(request.Colour);
        }

        protected override Task AdditionalValidation(UpsertServiceCategoryCommand request,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}