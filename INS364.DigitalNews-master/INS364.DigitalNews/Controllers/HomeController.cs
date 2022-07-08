using INS364.DigitalNews.Data;
using INS364.DigitalNews.Models;
using INS364.DigitalNews.ViewModels.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace INS364.DigitalNews.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> _logger { get; }

        private INewsRepository _newsData { get; }

        public HomeController(ILogger<HomeController> logger, INewsRepository newsData)
        {
            _logger = logger;
            _newsData = newsData;
        }

        public async Task<IActionResult> Index()
        {
            NewsSearchViewModel newsViewModel = new NewsSearchViewModel()
            {
                Keyword = null,
                NewsBriefings = await _newsData.GetNewsBrief()
            };

            return View("Index", newsViewModel);
        }

        public IActionResult About()
        {
            return View("About");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetNewsBriefWithKeyword(NewsSearchViewModel newsSearch)
        {
            if (!string.IsNullOrEmpty(newsSearch.Keyword))
            {
                NewsSearchViewModel newNewsSearch = new NewsSearchViewModel()
                {
                    Keyword = newsSearch.Keyword,
                    NewsBriefings = await _newsData.GetNewsBriefWithKeywordsOnTitle(newsSearch.Keyword)
                };

                return View("Index", newNewsSearch);
            }
            
            return await Index();
        }
    }
}