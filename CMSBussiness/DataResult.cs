using System.Collections.Generic;

namespace CRMBussiness
{
    public class DataResult<T>
    {
        public bool Error { get; set; } = false;
        public IList<T> Result { get; set; } = null;//for list item
        public T DataItem { get; set; } // for an complex item
    }
}