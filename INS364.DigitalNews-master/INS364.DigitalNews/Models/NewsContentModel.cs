using System;
using System.Collections.Generic;

#nullable disable

namespace INS364.DigitalNews.Models
{
    public partial class NewsContentModel
    {
        public NewsContentModel()
        {
            TblComments = new HashSet<CommentModel>();
        }

        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string NewsDesc { get; set; }
        public int NewsAuthorId { get; set; }
        public string NewsContent { get; set; }
        public int NewsTagId { get; set; }
        public DateTime NewsPublishDate { get; set; }
        public int NewsImpactId { get; set; }
        public int? NewsFileId { get; set; }

        public virtual UserModel NewsAuthor { get; set; }
        public virtual NewsFileModel NewsFile { get; set; }
        public virtual NewsImpactModel NewsImpact { get; set; }
        public virtual NewsTagModel NewsTag { get; set; }
        public virtual ICollection<CommentModel> TblComments { get; set; }
    }
}
