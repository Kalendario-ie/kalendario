using System;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Domain;

namespace Kalendario.Application.ResourceModel
{
    public class EmployeeResourceModel : IMapFrom<Employee>
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }
    }
}