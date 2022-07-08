namespace INS364.DigitalNews.ViewModels.News
{
    public class NewsBriefViewModel
    {
        public int NewsId { get; set; }

        public string NewsTitle { get; set; }

        public string NewsDescription { get; set; }

        public string NewsFile { get; set; }

        public string NewsTag { get; set; }

        public bool HasVideo { get; set; }

        public int ImpactGrade { get; set; }
    }
}