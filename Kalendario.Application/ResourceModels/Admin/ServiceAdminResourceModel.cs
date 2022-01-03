using System;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Admin
{
    public class ServiceAdminResourceModel : IMapFrom<Service>
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }
    }
}