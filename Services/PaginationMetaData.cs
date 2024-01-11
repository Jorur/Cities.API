namespace CityInfo.API.Services;

public class PaginationMetaData
{
    public int TotalItemCount { get; set; }
    public int TotalPagecount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }

    public PaginationMetaData(int totalItemCount, int pageSize, int currentPage)
    {
        TotalItemCount = totalItemCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPagecount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
    }
}