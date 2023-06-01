namespace SkyDiveTicketing.API.Base
{
    public interface IPaging<T>
    {
        IEnumerable<T> Data { get; set; }
        int TotalRecord { get; set; }
    }
    public class Paging<T> : IPaging<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalRecord { get; set; }
    }

    public static class Paging
    {
        public static IQueryable<T> ToPagingAndSorting<T>(this IQueryable<T> dataModels, IPageQuery objectQuery)
        {
            return objectQuery.PageSize == 0
                ? dataModels
                : dataModels
                    .Skip((objectQuery.PageIndex - 1) * objectQuery.PageSize)
                    .Take(objectQuery.PageSize);
        }

        public static IEnumerable<T> ToPagingAndSorting<T>(this IEnumerable<T> dataModels, IPageQuery objectQuery)
        {
            return objectQuery.PageSize == 0
                ? dataModels
                : dataModels
                    .Skip((objectQuery.PageIndex - 1) * objectQuery.PageSize)
                    .Take(objectQuery.PageSize);
        }

        public static Paging<T> ToPaging<T>(this IEnumerable<T> data, int totalRecord)
        {
            return new Paging<T>
            {
                Data = data,
                TotalRecord = totalRecord
            };
        }
    }
}
