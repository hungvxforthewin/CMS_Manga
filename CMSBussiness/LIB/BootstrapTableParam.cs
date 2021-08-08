using System;
using System.Collections.Generic;
using System.Text;

namespace CRMBussiness.LIB
{
    public class BootstrapTableParam
    {
        public int limit { get; set; }
        public int offset { get; set; }
        //asc-desc
        public string order { get; set; }
        public string search { get; set; }
        //column
        public string sort { get; set; }
        public int IDStudent { get; set; }
        public int pageNumber()
        {
            if (limit == 0)
            {
                return 0;
            }
            else
            {
                return ((offset / limit) + 1);
            }
        }
        public int pageSize()
        {
            return limit;
        }
    }
}
