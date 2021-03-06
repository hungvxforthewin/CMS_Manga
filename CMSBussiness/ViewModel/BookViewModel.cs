using System;
using System.Collections.Generic;
using System.Text;

namespace CMSBussiness.ViewModel
{
    public class BookViewModel
    {
        public int BookId { get; set; }

        public Guid BookUUID { get; set; }
        public string[] CategoryIds { get; set; }

        public string BookName { get; set; }

        public string BookDescription { get; set; }

        public byte adultLimit { get; set; }

        public byte bookSexId { get; set; }

        public decimal Rating { get; set; }

        public int LikeNo { get; set; }

        public int FollowNo { get; set; }

        public bool isEnable { get; set; }

        public bool commentAllowed { get; set; }

        public int authorAccountId { get; set; }

        public string CategoryIdsStr { get; set; }
    }
    public class SearchBookViewModel
    {
        public string Key { get; set; }
        public string CategoryId { get; set; }
        public string SexId { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string Status { get; set; }

        public int Size { get; set; } = 10;

        public int Page { get; set; } = 1;
    }
    public class DisplayBookViewModel
    {
        public int BookId { get; set; }

        public string BookName { get; set; }
        public string CategoryName { get; set; }

        public string ImgUrl { get; set; }
        public string Author { get; set; }

        public string Sex { get; set; }

        public decimal Rating { get; set; }

        public string Status { get; set; }
        public bool isEnable { get; set; }
        public bool IsApprove { get; set; }
    }
}
