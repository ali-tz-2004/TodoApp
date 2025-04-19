using TodoApp.Common.DtoHandler;

namespace TodoApp.Common.Utilities;

public static class ConvertTypes
{
    public static PaginationResponse<T> Paginate<T>(this IList<T> source, int pageNumber, int pageSize)
    {
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        
        var total = source.Count();

        return new PaginationResponse<T>()
        {
            Items = items,
            TotalCount = total,
            Page = pageNumber,
            PageSize = pageSize
        };
    }
}