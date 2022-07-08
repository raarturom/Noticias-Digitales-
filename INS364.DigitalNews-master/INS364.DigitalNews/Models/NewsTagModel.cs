using System;
using System.Collections.Generic;

#nullable disable

namespace INS364.DigitalNews.Models
{
    public partial class NewsTagModel
    {
        public NewsTagModel()
        {
            TblNews = new HashSet<NewsContentModel>();
        }

        public int NewsTagId { get; set; }
        public string NewsTagDesc { get; set; }

        public virtual ICollection<NewsContentModel> TblNews { get; set; }
    }
}
