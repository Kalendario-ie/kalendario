using System;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Domain;

namespace Kalendario.Application.ResourceModels.Admin
{
    public class ServiceCategoryAdminResourceModel : IMapFrom<ServiceCategory>
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string Name { get; set; }
    }
}