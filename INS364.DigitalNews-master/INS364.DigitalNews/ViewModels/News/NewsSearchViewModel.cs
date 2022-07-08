using System.Collections.Generic;

namespace INS364.DigitalNews.ViewModels.News
{
    public class NewsSearchViewModel
    {
        public string Keyword { get; set; }

        public List<NewsBriefViewModel> NewsBriefings { get; set; }
    }
}