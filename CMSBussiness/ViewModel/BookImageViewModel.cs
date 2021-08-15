using System;
using System.Collections.Generic;
using System.Text;

namespace CMSBussiness.ViewModel
{
    public class BookImageViewModel
    {
        public int BookId { get; set; }
        public int imgId { get; set; }

        public string imgUrl { get; set; }
        public bool IsBanner { get; set; }
    }

    public class SearchBookImageViewModel
    {
        public string Key { get; set; }
        public string IsBanner { get; set; }

        public int Size { get; set; } = 10;

        public int Page { get; set; } = 1;
    }

    public class DisplayBookImageViewModel
    {
        public int BookId { get; set; }

        public int imgId { get; set; }

        public string BookName { get; set; }
        public string imgUrl { get; set; }

        public string IsBanner { get; set; }
    }
}
