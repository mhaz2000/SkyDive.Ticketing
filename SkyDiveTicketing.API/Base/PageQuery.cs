namespace SkyDiveTicketing.API.Base
{
    public interface IPageQuery
    {
        int PageSize { get; set; }
        int PageIndex { get; set; }
        string? OrderBy { get; set; }
    }

    public class PageQuery : IPageQuery
    {
        public PageQuery()
        {
            PageSize = 10;
            PageIndex = 1;
            OrderBy = string.Empty;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string? OrderBy { get; set; }

    }
}
