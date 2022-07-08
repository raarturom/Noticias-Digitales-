using System;
using System.Collections.Generic;

#nullable disable

namespace INS364.DigitalNews.Models
{
    public partial class UserModel
    {
        public UserModel()
        {
            TblComments = new HashSet<CommentModel>();
            TblNews = new HashSet<NewsContentModel>();
        }

        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }

        public virtual ICollection<CommentModel> TblComments { get; set; }
        public virtual ICollection<NewsContentModel> TblNews { get; set; }
    }
}
