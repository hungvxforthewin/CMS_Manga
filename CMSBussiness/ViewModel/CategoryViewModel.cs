using System;
using System.Collections.Generic;
using System.Text;

namespace CMSBussiness.ViewModel
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public short ParentCategoryId { get; set; }

        public string ParentCategoryName { get; set; }

        public byte OrderNo { get; set; }

        public bool isActive { get; set; }

        public string Status { get; set; }
    }
    public class SearchCategoryViewModel
    {

        public string Key { get; set; }

        public string Status { get; set; }

        public int Size { get; set; } = 10;

        public int Page { get; set; } = 1;
    }
}
