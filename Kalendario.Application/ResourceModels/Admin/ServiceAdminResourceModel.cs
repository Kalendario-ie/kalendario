using System;
using System.ComponentModel.DataAnnotations;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Entities;

namespace Kalendario.Application.ResourceModels.Admin
{
    public class ServiceAdminResourceModel : IMapFrom<Service>
    {
        public Guid Id { get; set; }

        [Required] public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public TimeSpan Duration { get; set; }

        public Guid? ServiceCategoryId { get; set; }
    }
}