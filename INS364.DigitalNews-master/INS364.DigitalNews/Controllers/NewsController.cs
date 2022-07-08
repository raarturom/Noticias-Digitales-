using INS364.DigitalNews.Data;
using INS364.DigitalNews.Models;
using INS364.DigitalNews.ViewModels.Comment;
using INS364.DigitalNews.ViewModels.News;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace INS364.DigitalNews.Controllers
{
    // Controller used in the registration and getting of the news at the database in the API.
    public class NewsController : Controller
    {
        private INewsRepository _newsData { get; }

        public NewsController(INewsRepository newsData)
        {
            _newsData = newsData;
        }

        public IActionResult Index()
        {
            return View("Content");
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create()
        {
            await FillDropDownLists();
            ViewBag.Published = null;
            return View("Publish");
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Comment(int newsId)
        {
            ViewBag.CommentPublished = null;
            ViewBag.NewsId = newsId;
            return View("Comment");
        }

        private async Task FillDropDownLists()
        {
            List<SelectListItem> tags = new List<SelectListItem>();
            List<SelectListItem> impacts = new List<SelectListItem>();

            foreach (NewsImpactModel impact in await _newsData.GetNewsImpactGrades())
            {
                impacts.Add(new SelectListItem(impact.NewsImpactDesc, impact.NewsImpactId.ToString()));
            }

            foreach (NewsTagModel tag in await _newsData.GetNewsTags())
            {
                tags.Add(new SelectListItem(tag.NewsTagDesc, tag.NewsTagId.ToString()));
            }

            ViewBag.Tags = tags;
            ViewBag.Impacts = impacts;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RegisterNews(NewsPublishViewModel newsPublishInfo)
        {
            ViewBag.Published = false;
            if (ModelState.IsValid)
            {
                newsPublishInfo.NewsAuthor = User.Identity.Name;
                ViewBag.Published = await _newsData.RegisterNewsAsync(newsPublishInfo);
            }

            ModelState.Clear();
            await FillDropDownLists();
            return View("Publish");
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RegisterComment(CommentViewModel newsCommentInfo, int newsId)
        {
            ViewBag.CommentPublished = false;
            if (ModelState.IsValid)
            {
                newsCommentInfo.NewsId = newsId;
                newsCommentInfo.CommentAuthor = User.Identity.Name;
                ViewBag.CommentPublished = await _newsData.RegisterCommentAsync(newsCommentInfo);
                if (ViewBag.CommentPublished == true)
                {
                    return GetNewsDetails(newsCommentInfo.NewsId).Result;
                }
            }

            ModelState.Clear();
            return View("Comment");
        }

        public async Task<IActionResult> GetNewsDetails(int newsId)
        {
            NewsContentViewModel newsContent = await _newsData.GetNewsContent(newsId);
            return View("Content", newsContent);
        }
    }
}