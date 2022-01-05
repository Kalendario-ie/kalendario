using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kalendario.Application.Common.Extensions;

public static class IEnumerableExtensions
{
    public static async Task<bool> AnyAsync<TTask>(this IEnumerable<TTask> source) where TTask : Task<bool>
    {
        foreach (var element in source)
        {
            if (await element)
                return true;
        }

        return false;
    }
}