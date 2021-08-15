using System;
using System.Collections.Generic;
using System.Text;

namespace CMSBussiness.ViewModel
{
    public class BookChapterViewModel
    {
        public int BookId { get; set; }
        public Guid ChapterId { get; set; }

        public string ChapterName { get; set; }
        public string imgUrls { get; set; }
        public string imgUrlsEdit { get; set; }

        public byte adultLimit { get; set; }

        public int LikeNo { get; set; }

        public int CommentNo { get; set; }

        public byte ChapterStatus { get; set; }

        public string PublishDate { get; set; }

        //public DateTime lastUpdateTime { get; set; }

        public bool isPremium { get; set; }
    }
    public class SearchBookChapterViewModel
    {
        public string Key { get; set; }
        public string BookName { get; set; }
        public string ChapterStatus { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public int Size { get; set; } = 10;

        public int Page { get; set; } = 1;
    }
    public class DisplayBookChapterViewModel
    {
        public int BookId { get; set; }
        public Guid ChapterId { get; set; }
        public string BookName { get; set; }
        public string ChapterName { get; set; }
        public string NumberPages { get; set; }
        public string adultLimit { get; set; }
        public string PublishDate { get; set; }
        public string ChapterStatus { get; set; }
    }
}
