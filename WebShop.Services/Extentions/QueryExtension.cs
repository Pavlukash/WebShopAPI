using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Services.Extentions
{
    public static class QueryExtension
    {
        public static T FirstOrNotFound<T>(this IEnumerable<T> query)
        {
            var result = query.FirstOrDefault();
            if (result == null)
            {
                throw new Exception();
            }

            return result;
        }

       //
        public static async Task<T> FirstOrNotFoundAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken)
        {
            var result = await source.FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                throw new Exception();
            }

            return result;
        }
    }
}