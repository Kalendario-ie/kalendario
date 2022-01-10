using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;
using Kalendario.Core.ValueObject;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Customer), $"{Customer.CreateRole},{Customer.UpdateRole}")]
public class UpsertCustomerCommand : BaseUpsertAdminCommand<CustomerAdminResourceModel>
{
    public string Name { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public string Warning { get; set; }


    public class Handler : BaseUpsertAdminCommandHandler<UpsertCustomerCommand, Customer, CustomerAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserManager currentUserManager)
            : base(context, mapper, currentUserManager)
        {
        }

        protected override IQueryable<Customer> Entities => Context.Customers;

        protected override void UpdateDomain(Customer domain, UpsertCustomerCommand request)
        {
            domain.Name = request.Name;
            domain.PhoneNumber = request.PhoneNumber;
            domain.Email = request.Email;
            domain.Warning = request.Warning;
        }

        protected override Task AdditionalValidation(UpsertCustomerCommand request)
        {
            return Task.CompletedTask;
        }
    }
}