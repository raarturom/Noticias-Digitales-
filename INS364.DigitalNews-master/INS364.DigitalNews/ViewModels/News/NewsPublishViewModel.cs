using INS364.DigitalNews.Utility;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace INS364.DigitalNews.ViewModels.News
{
    public class NewsPublishViewModel : ErrorMessages
    {
        [Required(ErrorMessage = REQUIRED_MSG)]
        public string NewsTitle { get; set; }

        [Required(ErrorMessage = REQUIRED_MSG)]
        public string NewsDescription { get; set; }

        [Required(ErrorMessage = REQUIRED_MSG)]
        public string NewsContent { get; set; }

        [Required(ErrorMessage = REQUIRED_MSG)]
        public int NewsTagId { get; set; }
        
        [Required(ErrorMessage = REQUIRED_MSG)]
        public int NewsImpactGrade { get; set; }

        [AllowedExtensions(new string[] { 
            ".jpeg", ".jpg", ".png", ".gif", // Images
            ".mp4", ".webm" // Videos
        })]
        public IFormFile NewsFile { get; set; }

        public string NewsAuthor { get; set; }
    }
}