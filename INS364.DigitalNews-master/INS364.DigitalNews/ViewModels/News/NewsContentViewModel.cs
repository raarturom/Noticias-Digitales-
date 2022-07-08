using INS364.DigitalNews.ViewModels.Comment;
using System;
using System.Collections.Generic;

namespace INS364.DigitalNews.ViewModels.News
{
    public class NewsContentViewModel
    {
        public int NewsId { get; set; }

        public string NewsTitle { get; set; }

        public string NewsContent { get; set; }

        public string NewsDescription { get; set; }

        public string NewsTag { get; set; }

        public string NewsAuthor { get; set; }

        public DateTime NewsPublishDate { get; set; }

        public string NewsFileInfo { get; set; }

        public bool IsNewsFileVideo { get; set; }

        public List<CommentViewModel> NewsComments { get; set; }

        public List<NewsBriefViewModel> RelatedNews { get; set; }
    }
}