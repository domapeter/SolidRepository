namespace Solid.Models.Interfaces
{
    public interface IPageResult<T> where T : class
    {
        int CurrentPage { get; set; }
        int FirstPage { get; set; }
        int LastPage { get; set; }
        int NextPage { get; set; }
        int PreviousPage { get; set; }
        IList<T> Records { get; set; }
        int Size { get; set; }
        int TotalCount { get; set; }
    }
}