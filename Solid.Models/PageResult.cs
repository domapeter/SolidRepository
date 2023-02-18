using Solid.Models.Interfaces;

namespace Solid.Models;

public class PageResult<T> : IPageResult<T> where T : class
{
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int FirstPage { get; } = 1;
    public int LastPage => (int)Math.Ceiling((double)TotalCount / Size);
    public int NextPage => (CurrentPage + 1) <= LastPage ? (CurrentPage + 1) : -1;
    public int PreviousPage => (CurrentPage - 1) >= 1 ? (CurrentPage - 1) : -1;
    public int Size { get; set; }
    public IList<T> Records { get; set; }
}