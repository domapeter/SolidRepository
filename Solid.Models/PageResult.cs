using Solid.Models.Interfaces;

namespace Solid.Models;

public class PageResult<T> : IPageResult<T> where T : class
{
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int FirstPage { get; set; }
    public int LastPage { get; set; }
    public int NextPage { get; set; }
    public int PreviousPage { get; set; }
    public int Size { get; set; }
    public IList<T> Records { get; set; }
}