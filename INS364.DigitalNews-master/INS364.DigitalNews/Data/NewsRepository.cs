using INS364.DigitalNews.Context;
using INS364.DigitalNews.Models;
using INS364.DigitalNews.ViewModels.Comment;
using INS364.DigitalNews.ViewModels.News;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace INS364.DigitalNews.Data
{
    public class NewsRepository : INewsRepository
    {
        private IWebHostEnvironment _environment { get; }

        private NewsDbContext _context { get; }

        public NewsRepository(NewsDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<List<NewsTagModel>> GetNewsTags()
        {
            return await _context.NewsTags.ToListAsync();
        }

        public async Task<List<NewsImpactModel>> GetNewsImpactGrades()
        {
            return await _context.NewsImpacts.ToListAsync();
        }

        public async Task<List<NewsBriefViewModel>> GetNewsBrief()
        {
            List<NewsBriefViewModel> newsBrief = new List<NewsBriefViewModel>();
            foreach (NewsContentModel newsModel in await _context.News.ToListAsync())
            {
                NewsFileModel selectedNewsFile = _context.NewsFiles.Where(entity => entity.NewsFileId == newsModel.NewsFileId).FirstOrDefault();
                NewsTagModel selectedNewsTag = _context.NewsTags.Where(entity => entity.NewsTagId == newsModel.NewsTagId).FirstOrDefault();
                newsBrief.Add(new NewsBriefViewModel()
                {
                    NewsId = newsModel.NewsId,
                    NewsTitle = newsModel.NewsTitle,
                    NewsDescription = newsModel.NewsDesc,                    
                    NewsFile = selectedNewsFile != null ? selectedNewsFile.NewsFilePath : null,
                    NewsTag = selectedNewsTag.NewsTagDesc,
                    HasVideo = selectedNewsFile != null ? selectedNewsFile.IsVideo.Value : false,
                    ImpactGrade = newsModel.NewsImpactId
                });
            }

            return newsBrief.OrderByDescending(news => news.ImpactGrade).ToList();
        }

        public async Task<NewsContentViewModel> GetNewsContent(int newsId)
        {
            NewsContentModel news = _context.News.Where(entity => entity.NewsId == newsId).FirstOrDefault();
            if (news != null)
            {
                List<CommentViewModel> comments = new List<CommentViewModel>();
                foreach (CommentModel comment in await _context.Comments.Where(entity => entity.NewsId == newsId).ToListAsync())
                {
                    UserModel commenter = _context.Users.Where(entity => entity.UserId == comment.CommentUserId).FirstOrDefault();
                    comments.Add(new CommentViewModel()
                    {
                        NewsId = newsId,
                        CommentAuthor = string.Join(' ', new string[] { commenter.Firstname, commenter.Lastname }),
                        CommentContent = comment.CommentBody,
                        CommentPublishDate = comment.CommentPublishDate,
                    });
                }

                UserModel newsAuthor = _context.Users.Where(entity => entity.UserId == news.NewsAuthorId).FirstOrDefault();
                NewsTagModel newsTag = _context.NewsTags.Where(entity => entity.NewsTagId == news.NewsTagId).FirstOrDefault();
                NewsFileModel newsFile = _context.NewsFiles.Where(entity => entity.NewsFileId == news.NewsFileId).FirstOrDefault();
                List<NewsBriefViewModel> relatedNews = new List<NewsBriefViewModel>();
                foreach (NewsContentModel newsModel in await _context.News.Where(entity => entity.NewsTagId == news.NewsTagId).ToListAsync())
                {
                    NewsFileModel selectedNewsFile = _context.NewsFiles.Where(entity => entity.NewsFileId == newsModel.NewsFileId).FirstOrDefault();
                    NewsTagModel selectedNewsTag = _context.NewsTags.Where(entity => entity.NewsTagId == newsModel.NewsTagId).FirstOrDefault();
                    relatedNews.Add(new NewsBriefViewModel()
                    {
                        NewsId = newsModel.NewsId,
                        NewsTitle = newsModel.NewsTitle,
                        NewsDescription = newsModel.NewsDesc,
                        NewsFile = selectedNewsFile != null ? selectedNewsFile.NewsFilePath : null,
                        NewsTag = selectedNewsTag.NewsTagDesc,
                        HasVideo = selectedNewsFile != null ? selectedNewsFile.IsVideo.Value : false,
                        ImpactGrade = newsModel.NewsImpactId
                    });
                }

                int index = relatedNews.FindIndex(news => news.NewsId == newsId);
                relatedNews.RemoveAt(index);

                NewsContentViewModel newsContent = new NewsContentViewModel()
                {
                    NewsId = news.NewsId,
                    NewsAuthor = string.Join(' ', new string[] { newsAuthor.Firstname, newsAuthor.Lastname }),
                    NewsContent = news.NewsContent,
                    NewsPublishDate = news.NewsPublishDate,
                    NewsComments = comments,
                    NewsTag = newsTag.NewsTagDesc,
                    NewsTitle = news.NewsTitle,
                    NewsFileInfo = newsFile != null ? newsFile.NewsFilePath : null,
                    NewsDescription = news.NewsDesc,
                    IsNewsFileVideo = newsFile != null ? newsFile.IsVideo.Value : false,
                    RelatedNews = relatedNews
                };

                return newsContent;
            }

            return null;
        }

        public async Task<bool> RegisterNewsAsync(NewsPublishViewModel news)
        {
            bool success = false;
            UserModel author = _context.Users.Where(entity => entity.Username == news.NewsAuthor).FirstOrDefault();

            try
            {
                if (news.NewsFile != null)
                {
                    NewsFileModel newsFile = new NewsFileModel()
                    {
                        // Relative path is used to save it into the database.
                        NewsFilePath = Path.Combine("uploads", news.NewsFile.FileName),
                        IsVideo = news.NewsFile.FileName.EndsWith(".mp4") ||
                            news.NewsFile.FileName.EndsWith(".webm") 
                    };

                    // Complete path is used to save it on disk.
                    string contentPath = Path.Combine(_environment.ContentRootPath, _environment.WebRootPath, "uploads", news.NewsFile.FileName);
                    using (FileStream stream = new FileStream(contentPath, FileMode.Create))
                    {
                        if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, _environment.WebRootPath, "uploads")))
                        {
                            Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, _environment.WebRootPath, "uploads"));
                        }

                        await news.NewsFile.CopyToAsync(stream);
                        stream.Flush();
                    }

                    _context.NewsFiles.Add(newsFile);
                    await _context.SaveChangesAsync();
                    NewsContentModel newsModel = new NewsContentModel()
                    {
                        NewsTitle = news.NewsTitle,
                        NewsDesc = news.NewsDescription,
                        NewsContent = news.NewsContent,
                        NewsAuthorId = author.UserId,
                        NewsImpactId = news.NewsImpactGrade,
                        NewsTagId = news.NewsTagId,
                        NewsFileId = newsFile.NewsFileId
                    };

                    _context.News.Add(newsModel);
                }

                else
                {
                    NewsContentModel newsModel = new NewsContentModel()
                    {
                        NewsTitle = news.NewsTitle,
                        NewsDesc = news.NewsDescription,
                        NewsContent = news.NewsContent,
                        NewsAuthorId = author.UserId,
                        NewsImpactId = news.NewsImpactGrade,
                        NewsTagId = news.NewsTagId,
                        NewsFileId = null
                    };

                    _context.News.Add(newsModel);
                }

                await _context.SaveChangesAsync();
                success = true;
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return success;
        }

        public async Task<bool> RegisterCommentAsync(CommentViewModel comment)
        {
            bool success = false;

            try
            {
                NewsContentModel news = _context.News.Where(entity => entity.NewsId == comment.NewsId).FirstOrDefault();
                if (news != null)
                {
                    UserModel commenterModel = _context.Users.Where(entity => entity.Username == comment.CommentAuthor).FirstOrDefault();
                    CommentModel commentModel = new CommentModel()
                    {
                        CommentUserId = commenterModel.UserId,
                        CommentBody = comment.CommentContent,
                        NewsId = comment.NewsId,
                    };

                    _context.Comments.Add(commentModel);
                    await _context.SaveChangesAsync();
                    success = true;
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return success;
        }

        public async Task<List<NewsBriefViewModel>> GetNewsBriefWithKeywordsOnTitle(string keyword)
        {
            List<NewsBriefViewModel> newsBrief = new List<NewsBriefViewModel>();
            foreach (NewsContentModel newsModel in await _context.News.Where(entity => entity.NewsTitle.Contains(keyword)).ToListAsync())
            {
                NewsFileModel selectedNewsFile = _context.NewsFiles.Where(entity => entity.NewsFileId == newsModel.NewsFileId).FirstOrDefault();
                NewsTagModel selectedNewsTag = _context.NewsTags.Where(entity => entity.NewsTagId == newsModel.NewsTagId).FirstOrDefault();
                newsBrief.Add(new NewsBriefViewModel()
                {
                    NewsId = newsModel.NewsId,
                    NewsTitle = newsModel.NewsTitle,
                    NewsDescription = newsModel.NewsDesc,
                    NewsFile = selectedNewsFile != null ? selectedNewsFile.NewsFilePath : null,
                    NewsTag = selectedNewsTag.NewsTagDesc,
                    HasVideo = selectedNewsFile != null ? selectedNewsFile.IsVideo.Value : false,
                    ImpactGrade = newsModel.NewsImpactId
                });
            }

            return newsBrief.OrderByDescending(news => news.ImpactGrade).ToList();
        }
    }
}