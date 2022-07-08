using System;
using System.Collections.Generic;

#nullable disable

namespace INS364.DigitalNews.Models
{
    public partial class NewsFileModel
    {
        public NewsFileModel()
        {
            TblNews = new HashSet<NewsContentModel>();
        }

        public int NewsFileId { get; set; }
        public string NewsFilePath { get; set; }
        public bool? IsVideo { get; set; }

        public virtual ICollection<NewsContentModel> TblNews { get; set; }
    }
}
