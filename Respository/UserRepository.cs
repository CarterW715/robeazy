using RobeazyCore.Data;
using RobeazyCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobeazyCore.Respository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AttachUser(ApplicationUser user)
        {
            _context.Users.Attach(user);
        }

        public void SaveUserChanges()
        {
            _context.SaveChangesAsync();
        }

        public ApplicationUser GetUserById(string id) 
            => _context.Users.Find(id);

        public ApplicationUser GetUserByUserName(string userName) 
            => _context.Users.Where(u => u.UserName.ToUpper() == userName.ToUpper()).FirstOrDefault();

        public List<UserContributionViewModel> GetContributionsByUser(ApplicationUser user)
        {
            List<UserContributionViewModel> Contributions = (from contrib in _context.Contributions
                                                             where contrib.Contributor.Id == user.Id
                                                             orderby contrib.CreateDate descending
                                                             select new UserContributionViewModel
                                                             {
                                                                 ContributionId = contrib.Id,
                                                                 StoryName = contrib.Story.Title,
                                                                 UserName = user.UserName,
                                                                 Upvotes = contrib.Upvotes,
                                                                 Downvotes = contrib.Downvotes,
                                                                 CreateDate = contrib.CreateDate
                                                             }).ToList();
            return Contributions;
        }

        public ContributionLike GetUserContributionLikeByUserByContribution(ApplicationUser user, Contribution contribution)
        {
            var userLike = (from like in _context.ContributionLikes
                            where like.User.Id == user.Id && like.ContributionId == contribution.Id
                            select like).FirstOrDefault();

            return userLike;
        }

        public StoryLike GetUserStoryLikeByUserByStory(ApplicationUser user, Story story)
        {
            var userLike = (from like in _context.StoryLikes
                            where like.User.Id == user.Id && like.StoryId == story.Id
                            select like).FirstOrDefault();

            return userLike;
        }

        public List<ContributionLikeViewModel> GetUserLikedContributions(ApplicationUser user)
        {
            var userLikes = (from like in _context.ContributionLikes
                             where like.User.Id == user.Id && like.Enabled
                             select new ContributionLikeViewModel
                             {
                                 ContributionId = like.ContributionId,
                                 StoryName = like.Contribution.Story.Title,
                                 UserName = user.UserName,
                                 Upvotes = like.Contribution.Upvotes,
                                 Downvotes = like.Contribution.Downvotes,
                                 CreateDate = like.Contribution.CreateDate,
                                 Liked = true,
                                 LikedDate = like.CreateDate
                             }).ToList();

            return userLikes;
        }

        public List<StoryViewModel> GetUserLikedStories(ApplicationUser user)
        {
            var userLikes = (from like in _context.StoryLikes
                             where like.User.Id == user.Id && like.Enabled
                             select new StoryViewModel
                             {
                                 StoryId = like.StoryId,
                                 Title = like.Story.Title,
                                 Author = like.Story.Author.UserName,
                                 Upvotes = like.Story.Upvotes,
                                 Downvotes = like.Story.Downvotes,
                                 CreateDate = like.Story.CreateDate,
                                 Liked = true,
                                 LikedDate = like.CreateDate
                             }).ToList();

            return userLikes;
        }

        public void AddContributionLike(ContributionLike like)
        {
            try
            {
                _context.ContributionLikes.Add(like);
            }
            catch (Exception ex)
            {
                // add logger
            }
        }

        public ContributionLike EnableContributionLike(ContributionLike like)
        {
            _context.ContributionLikes.Attach(like);
            like.Enabled = true;
            _context.SaveChangesAsync();

            return like;
        }

        public ContributionLike DisableContributionLike(ContributionLike like)
        {
            _context.ContributionLikes.Attach(like);
            like.Enabled = false;
            _context.SaveChangesAsync();

            return like;
        }

        public void AddStoryLike(StoryLike like)
        {
            try
            {
                _context.StoryLikes.Add(like);
            }
            catch (Exception ex)
            {
                // add logger
            }
        }

        public StoryLike EnableStoryLike(StoryLike like)
        {
            _context.StoryLikes.Attach(like);
            like.Enabled = true;
            _context.SaveChangesAsync();

            return like;
        }

        public StoryLike DisableStoryLike(StoryLike like)
        {
            _context.StoryLikes.Attach(like);
            like.Enabled = false;
            _context.SaveChangesAsync();

            return like;
        }

        public Report GetContributionReportByUserByContribution(ApplicationUser user, Contribution contribution)
        {
            var textReport = (from report in _context.Reports
                              where report.User.Id == user.Id && report.ContributionId == contribution.Id
                              select report).FirstOrDefault();

            return textReport;
        }

        public Report GetStoryReportByUserByStory(ApplicationUser user, Story story)
        {
            var textReport = (from report in _context.Reports
                              where report.User.Id == user.Id && report.StoryId == story.Id
                              select report).FirstOrDefault();

            return textReport;
        }

        public void AddUserReport(Report report)
        {
            _context.Reports.Add(report);
        }
    }
}