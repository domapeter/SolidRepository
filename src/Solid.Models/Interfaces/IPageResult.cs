namespace Solid.Models.Interfaces;

public interface IPageResult<T> where T : class
{
    int CurrentPage { get; set; }
    int FirstPage { get; }
    int LastPage { get; }
    int NextPage { get; }
    int PreviousPage { get; }
    IList<T> Records { get; set; }
    int Size { get; set; }
    int TotalCount { get; set; }
}