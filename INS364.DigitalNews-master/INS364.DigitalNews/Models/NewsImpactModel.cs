using System;
using System.Collections.Generic;

#nullable disable

namespace INS364.DigitalNews.Models
{
    public partial class NewsImpactModel
    {
        public NewsImpactModel()
        {
            TblNews = new HashSet<NewsContentModel>();
        }

        public int NewsImpactId { get; set; }
        public string NewsImpactDesc { get; set; }

        public virtual ICollection<NewsContentModel> TblNews { get; set; }
    }
}
