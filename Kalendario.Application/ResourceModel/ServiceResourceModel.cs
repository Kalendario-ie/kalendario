using System;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Domain;

namespace Kalendario.Application.ResourceModel
{
    public class ServiceResourceModel : IMapFrom<Service>
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }
    }
}