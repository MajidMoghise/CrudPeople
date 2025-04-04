using Helpers.FilterSearch;
using Microsoft.EntityFrameworkCore;

namespace CrudPeople.Infrastructure.EfCore.Repositories.Base.Queries
{
    internal static class EfPaging
    {
        internal static SearchResponseModel<TData> ToPagedList<TData>(this IEnumerable<TData> source, int pageIndex, int pageSize) 
        {
            if (source is IQueryable<TData> querable)
            {
                var PageIndex = pageIndex;
                var PageSize = pageSize;
                var TotalCount = querable.Count();
                var IndexFrom = (pageIndex - 1) * pageSize + 1;

                var TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

                var resultData = querable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                return new SearchResponseModel<TData>
                {
                    Page = PageIndex,
                    Data = resultData,
                    TotalCount = TotalCount,
                    TotalOfPages = TotalPages,
                };
            }
            else
            {
                var PageIndex = pageIndex;
                var PageSize = pageSize;
                var IndexFrom = (pageIndex - 1) * pageSize + 1;
                var TotalCount = source.Count();
                var TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

                var resultData = source.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                return new SearchResponseModel<TData>
                {
                    Page = PageIndex,
                    Data = resultData,
                    TotalCount = TotalCount,
                    TotalOfPages = TotalPages,
                };
            }
        }
        internal static async Task<SearchResponseModel<TData>> ToPagedListAsync<TData>(this IEnumerable<TData> source, int pageIndex, int pageSize, CancellationToken cancellationToken = default) 
        {
            if (source is IQueryable<TData> querable)
            {
                var PageIndex = pageIndex;
                var PageSize = pageSize;
                var TotalCount = querable.Count();

                var TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

                var resultData = await querable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync(cancellationToken);
                return new SearchResponseModel<TData>
                {
                    Page = PageIndex,
                    Data = resultData,
                    TotalCount = TotalCount,
                    TotalOfPages = TotalPages,
                };
            }
            else
            {
                var PageIndex = pageIndex;
                var PageSize = pageSize;
                var TotalCount = source.Count();
                var TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

                var resultData = source.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                return new SearchResponseModel<TData>
                {
                    Page = PageIndex,
                    Data = resultData,
                    TotalCount = TotalCount,
                    TotalOfPages = TotalPages,
                };
            }
        }
    }
}
