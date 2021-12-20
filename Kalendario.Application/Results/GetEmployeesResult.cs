using System.Collections.Generic;
using Kalendario.Application.ResourceModel;

namespace Kalendario.Application.Results
{
    public class GetEmployeesResult
    {
        public List<EmployeeResourceModel> Employees { get; set; }
    }
}