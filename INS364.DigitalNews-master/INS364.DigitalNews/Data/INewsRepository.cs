using INS364.DigitalNews.Models;
using INS364.DigitalNews.ViewModels.Comment;
using INS364.DigitalNews.ViewModels.News;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace INS364.DigitalNews.Data
{
    public interface INewsRepository
    {
        Task<bool> RegisterNewsAsync(NewsPublishViewModel news);

        Task<List<NewsBriefViewModel>> GetNewsBrief();

        Task<NewsContentViewModel> GetNewsContent(int newsId);

        Task<List<NewsTagModel>> GetNewsTags();

        Task<List<NewsImpactModel>> GetNewsImpactGrades();

        Task<bool> RegisterCommentAsync(CommentViewModel comment);

        Task<List<NewsBriefViewModel>> GetNewsBriefWithKeywordsOnTitle(string keywords);
    }
}