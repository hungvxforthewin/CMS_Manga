namespace CRMSite.Models
{
    public class PaginationBase
    {
        public int NumPage { get; set; }
        public int NoPage { get; set; }
        public int PageSize { get; set; }
        public bool HaveNextPage { get; set; }
        public bool HavePreviousPage { get; set; }
        public int PageCurrent { get; set; }
        public int PageStart { get; set; }
        
    }
}