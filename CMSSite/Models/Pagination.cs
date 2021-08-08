using System.Collections.Generic;

namespace CRMSite.Models
{
    public class Pagination<T> : PaginationBase where T : class
    {
        public IList<T> Items { get; set; }
        public IList<T> Results { get; set; }
    }
}