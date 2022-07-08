using INS364.DigitalNews.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace INS364.DigitalNews.ViewModels.Comment
{
    public class CommentViewModel : ErrorMessages
    {
        public int NewsId { get; set; }

        public string CommentAuthor { get; set; }

        public DateTime CommentPublishDate { get; set; }

        [Required(ErrorMessage = REQUIRED_MSG)]
        public string CommentContent { get; set; }
    }
}