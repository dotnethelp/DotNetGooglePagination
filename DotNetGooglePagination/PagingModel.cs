using System.Linq;

namespace DotNetGooglePagination
{
    public class PagingModel<T>
    {
        public IQueryable<T> DataList { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }
}
