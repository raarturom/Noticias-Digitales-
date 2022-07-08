using System;
using System.Collections.Generic;

#nullable disable

namespace INS364.DigitalNews.Models
{
    public partial class CommentModel
    {
        public int CommentId { get; set; }
        public int NewsId { get; set; }
        public int CommentUserId { get; set; }
        public string CommentBody { get; set; }
        public DateTime CommentPublishDate { get; set; }

        public virtual UserModel CommentUser { get; set; }
        public virtual NewsContentModel News { get; set; }
    }
}
