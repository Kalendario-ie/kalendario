using System.Collections.Generic;

namespace Kalendario.Application.Results
{
    public class GetAllResult<T>
    {
        public List<T> Entities { get; set; }
    }
}