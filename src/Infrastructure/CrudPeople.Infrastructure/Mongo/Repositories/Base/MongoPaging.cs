using Helpers.FilterSearch;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudPeople.Infrastructure.Mongo.Repositories.Base
{
    internal static class MongoPaging
    {
        internal static async Task<SearchResponseModel<TData>> ToPagedListAsync<TData>(
            this IMongoCollection<TData> collection,
            FilterDefinition<TData> filter,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var resultData = await collection
                .Find(filter)
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            return new SearchResponseModel<TData>
            {
                Page = pageIndex,
                Data = resultData,
                TotalCount = (int)totalCount,
                TotalOfPages = totalPages,
            };
        }
    }
}
