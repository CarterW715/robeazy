using RobeazyCore.Data;
using RobeazyCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobeazyCore.Respository.Text
{
    public class GuitarTabRepository : IRobeazyRepository<GuitarTab, GuitarTabViewModel, GuitarTabContribution, GuitarTabContributionView>
    {
        private readonly ApplicationDbContext _dB;

        public GuitarTabRepository(ApplicationDbContext context)
        {
            _dB = context;
        }

        public void AddStory(GuitarTab story, GuitarTabContribution firstContribution)
        {
            _dB.GuitarTabs.Add(story);
            AddContribution(firstContribution);
        }

        public GuitarTab GetStoryByStoryId(int storyId)
            => _dB.GuitarTabs.FirstOrDefault(s => s.StoryId == storyId);

        public GuitarTabViewModel GetStoryViewByTitle(string title, ApplicationUser user = null)
        {
            GuitarTabViewModel guitarTab;
            if (user == null)
            {
                guitarTab = (from tab in _dB.GuitarTabs
                             where tab.Story.Title == title
                             select new GuitarTabViewModel
                             {
                                 Story = tab.Story,
                                 Tab = tab,
                                 Liked = false,
                                 Reported = false,
                             }).FirstOrDefault();
            }
            else
            {
                guitarTab = (from tab in _dB.GuitarTabs
                             join like in _dB.StoryLikes on new { storyId = tab.StoryId, userId = user.Id } equals
                                                            new { storyId = like.StoryId, userId = like.UserId } into storyLikes
                             from storyLike in storyLikes.DefaultIfEmpty()
                             join report in _dB.Reports on new { storyId = (int?)tab.StoryId, userId = user.Id } equals
                                                           new { storyId = report.StoryId, userId = report.UserId } into storyReports
                             from storyReport in storyReports.DefaultIfEmpty()
                             where tab.Story.Title == title
                             select new GuitarTabViewModel
                             {
                                 Story = tab.Story,
                                 Tab = tab,
                                 Liked = storyLike != null && storyLike.UserId == user.Id && storyLike.Enabled,
                                 LikedDate = storyLike.CreateDate,
                                 Reported = storyReport != null && storyReport.UserId == user.Id,
                                 ReportDate = storyReport.CreateDate
                             }).FirstOrDefault();
            }

            return guitarTab;
        }

        public void AddContribution(GuitarTabContribution contribution)
        {
            _dB.GuitarTabContributions.Add(contribution);
            _dB.SaveChanges();
        }

        public GuitarTabContribution GetContributionById(int id)
            => _dB.GuitarTabContributions.Where(c => c.ContributionId == id).FirstOrDefault();

        public GuitarTabContributionView GetContributionViewById(int id, ApplicationUser user = null)
        {
            GuitarTabContributionView contribution;
            if (user == null)
            {
                contribution = (from contrib in _dB.GuitarTabContributions
                                where contrib.ContributionId == id
                                select new GuitarTabContributionView
                                {
                                    ContributionId = contrib.ContributionId,
                                    PreviousContributionId = null,
                                    UserName = contrib.Contribution.Contributor.UserName,
                                    Upvotes = contrib.Contribution.Upvotes,
                                    Downvotes = contrib.Contribution.Downvotes,
                                    CreateDate = contrib.Contribution.CreateDate,
                                    Liked = false,
                                    Reported = false,
                                    GuitarOne = contrib.GuitarOne,
                                    GuitarTwo = contrib.GuitarTwo
                                }).FirstOrDefault();
            }
            else
            {
                contribution = (from contrib in _dB.GuitarTabContributions
                                join like in _dB.ContributionLikes on new { contributionId = contrib.ContributionId, userId = user.Id } equals
                                                                     new { contributionId = like.ContributionId, userId = like.UserId } into contribLikes
                                from contribLike in contribLikes.DefaultIfEmpty()
                                join report in _dB.Reports on new { contributionId = (int?)contrib.ContributionId, userId = user.Id } equals
                                                             new { contributionId = report.ContributionId, userId = report.UserId } into contribReports
                                from contribReport in contribReports.DefaultIfEmpty()
                                where contrib.ContributionId == id
                                orderby contrib.Contribution.CreateDate ascending
                                select new GuitarTabContributionView
                                {
                                    ContributionId = contrib.ContributionId,
                                    PreviousContributionId = null,
                                    UserName = contrib.Contribution.Contributor.UserName,
                                    Upvotes = contrib.Contribution.Upvotes,
                                    Downvotes = contrib.Contribution.Downvotes,
                                    CreateDate = contrib.Contribution.CreateDate,
                                    Liked = contribLike != null && contribLike.UserId == user.Id && contribLike.Enabled,
                                    LikedDate = contribLike.CreateDate,
                                    Reported = contribReport != null && contribReport.UserId == user.Id,
                                    ReportDate = contribReport.CreateDate,
                                    GuitarOne = contrib.GuitarOne,
                                    GuitarTwo = contrib.GuitarTwo
                                }).FirstOrDefault();
            }

            return contribution;
        }

        public GuitarTabContributionView GetFirstContributionView(int storyId, ApplicationUser user = null)
        {
            GuitarTabContributionView firstContribution;
            if (user == null)
            {
                firstContribution = (from contrib in _dB.GuitarTabContributions
                                     where contrib.Contribution.StoryId == storyId
                                     orderby contrib.Contribution.CreateDate ascending
                                     select new GuitarTabContributionView
                                     {
                                         ContributionId = contrib.ContributionId,
                                         PreviousContributionId = null,
                                         UserName = contrib.Contribution.Contributor.UserName,
                                         Upvotes = contrib.Contribution.Upvotes,
                                         Downvotes = contrib.Contribution.Downvotes,
                                         CreateDate = contrib.Contribution.CreateDate,
                                         Liked = false,
                                         Reported = false,
                                         GuitarOne = contrib.GuitarOne,
                                         GuitarTwo = contrib.GuitarTwo
                                     }).FirstOrDefault();
            }
            else
            {
                firstContribution = (from contrib in _dB.GuitarTabContributions
                                     join like in _dB.ContributionLikes on new { contributionId = contrib.ContributionId, userId = user.Id } equals
                                                                          new { contributionId = like.ContributionId, userId = like.UserId } into contribLikes
                                     from contribLike in contribLikes.DefaultIfEmpty()
                                     join report in _dB.Reports on new { contributionId = (int?)contrib.ContributionId, userId = user.Id } equals
                                                                  new { contributionId = report.ContributionId, userId = report.UserId } into contribReports
                                     from contribReport in contribReports.DefaultIfEmpty()
                                     where contrib.Contribution.StoryId == storyId
                                     orderby contrib.Contribution.CreateDate ascending
                                     select new GuitarTabContributionView
                                     {
                                         ContributionId = contrib.ContributionId,
                                         PreviousContributionId = null,
                                         UserName = contrib.Contribution.Contributor.UserName,
                                         Upvotes = contrib.Contribution.Upvotes,
                                         Downvotes = contrib.Contribution.Downvotes,
                                         CreateDate = contrib.Contribution.CreateDate,
                                         Liked = contribLike != null && contribLike.User.Id == user.Id && contribLike.Enabled,
                                         LikedDate = contribLike.CreateDate,
                                         Reported = contribReport != null && contribReport.User.Id == user.Id,
                                         ReportDate = contribReport.CreateDate,
                                         GuitarOne = contrib.GuitarOne,
                                         GuitarTwo = contrib.GuitarTwo
                                     }).FirstOrDefault();
            }

            return firstContribution;
        }

        public GuitarTabContributionView GetNextHighestScoringContributionView(int contributionId, ApplicationUser user = null)
        {
            GuitarTabContributionView highestContribution;
            if (user == null)
            {
                highestContribution = (from contrib in _dB.GuitarTabContributions
                                       where contrib.PreviousContributionId == contributionId
                                       orderby contrib.Contribution.Upvotes descending, contrib.Contribution.CreateDate ascending
                                       select new GuitarTabContributionView
                                       {
                                           ContributionId = contrib.ContributionId,
                                           PreviousContributionId = contrib.PreviousContributionId,
                                           UserName = contrib.Contribution.Contributor.UserName,
                                           Upvotes = contrib.Contribution.Upvotes,
                                           Downvotes = contrib.Contribution.Downvotes,
                                           CreateDate = contrib.Contribution.CreateDate,
                                           Liked = false,
                                           Reported = false,
                                           GuitarOne = contrib.GuitarOne,
                                           GuitarTwo = contrib.GuitarTwo
                                       }).FirstOrDefault();
            }
            else
            {
                highestContribution = (from contrib in _dB.GuitarTabContributions
                                       join like in _dB.ContributionLikes on new { contributionId = contrib.ContributionId, userId = user.Id } equals
                                                                            new { contributionId = like.ContributionId, userId = like.User.Id } into contribLikes
                                       from contribLike in contribLikes.DefaultIfEmpty()
                                       join report in _dB.Reports on new { contributionId = (int?)contrib.ContributionId, userId = user.Id } equals
                                                                    new { contributionId = report.ContributionId, userId = report.User.Id } into contribReports
                                       from contribReport in contribReports.DefaultIfEmpty()
                                       where contrib.PreviousContributionId == contributionId
                                       orderby contrib.Contribution.Upvotes descending, contrib.Contribution.CreateDate ascending
                                       select new GuitarTabContributionView
                                       {
                                           ContributionId = contrib.ContributionId,
                                           PreviousContributionId = contrib.PreviousContributionId != null ? contrib.ContributionId : (int?)null,
                                           UserName = contrib.Contribution.Contributor.UserName,
                                           Upvotes = contrib.Contribution.Upvotes,
                                           Downvotes = contrib.Contribution.Downvotes,
                                           CreateDate = contrib.Contribution.CreateDate,
                                           Liked = contribLike != null && contribLike.UserId == user.Id && contribLike.Enabled,
                                           LikedDate = contribLike.CreateDate,
                                           Reported = contribReport != null && contribReport.UserId == user.Id,
                                           ReportDate = contribReport.CreateDate,
                                           GuitarOne = contrib.GuitarOne,
                                           GuitarTwo = contrib.GuitarTwo
                                       }).FirstOrDefault();
            }

            return highestContribution;
        }

        public List<GuitarTabContributionView> GetContributionReplies(int currentContributionId, int targetContributionId, int[] idBlackList, int recordCount, bool getRelatedReplies, ApplicationUser user = null)
        {
            List<GuitarTabContributionView> contributionReplies;
            if (user == null)
            {
                contributionReplies = (from contrib in _dB.GuitarTabContributions
                                       where contrib.PreviousContributionId == targetContributionId
                                       && !idBlackList.Contains(contrib.ContributionId)
                                       orderby contrib.Contribution.Upvotes descending, contrib.Contribution.CreateDate ascending
                                       select new GuitarTabContributionView
                                       {
                                           ContributionId = contrib.ContributionId,
                                           PreviousContributionId = contrib.ContributionId,
                                           UserName = contrib.Contribution.Contributor.UserName,
                                           Upvotes = contrib.Contribution.Upvotes,
                                           Downvotes = contrib.Contribution.Downvotes,
                                           CreateDate = contrib.Contribution.CreateDate,
                                           Liked = false,
                                           Reported = false,
                                           GuitarOne = contrib.GuitarOne,
                                           GuitarTwo = contrib.GuitarTwo
                                       }).Take(recordCount + 1).ToList();
            }
            else
            {
                contributionReplies = (from contrib in _dB.GuitarTabContributions
                                       join like in _dB.ContributionLikes on new { contributionId = contrib.ContributionId, userId = user.Id } equals
                                                                            new { contributionId = like.ContributionId, userId = like.User.Id } into contribLikes
                                       from contribLike in contribLikes.DefaultIfEmpty()
                                       join report in _dB.Reports on new { contributionId = (int?)contrib.ContributionId, userId = user.Id } equals
                                                                    new { contributionId = report.ContributionId, userId = report.User.Id } into contribReports
                                       from contribReport in contribReports.DefaultIfEmpty()
                                       where contrib.PreviousContributionId == targetContributionId
                                       && !idBlackList.Contains(contrib.ContributionId)
                                       orderby contrib.Contribution.Upvotes descending, contrib.Contribution.CreateDate ascending
                                       select new GuitarTabContributionView
                                       {
                                           ContributionId = contrib.ContributionId,
                                           PreviousContributionId = contrib.PreviousContribution != null ? contrib.PreviousContributionId : (int?)null,
                                           UserName = contrib.Contribution.Contributor.UserName,
                                           Upvotes = contrib.Contribution.Upvotes,
                                           Downvotes = contrib.Contribution.Downvotes,
                                           CreateDate = contrib.Contribution.CreateDate,
                                           Liked = contribLike != null && contribLike.UserId == user.Id && contribLike.Enabled,
                                           LikedDate = contribLike.CreateDate,
                                           Reported = contribReport != null && contribReport.UserId == user.Id,
                                           ReportDate = contribReport.CreateDate,
                                           GuitarOne = contrib.GuitarOne,
                                           GuitarTwo = contrib.GuitarTwo
                                       }).Take(recordCount + 1).ToList();
            }

            return getRelatedReplies ? ProcessContributionModels(currentContributionId, contributionReplies, recordCount, user) : contributionReplies;
        }

        public List<GuitarTabContributionView> GetContributionViewsByUser(ApplicationUser user)
        {
            List<GuitarTabContributionView> contributionReplies = (from contrib in _dB.GuitarTabContributions
                                                                   where contrib.Contribution.ContributorId == user.Id
                                                                   orderby contrib.Contribution.CreateDate descending
                                                                   select new GuitarTabContributionView
                                                                   {
                                                                       ContributionId = contrib.ContributionId,
                                                                       StoryName = contrib.Contribution.Story.Title,
                                                                       UserName = user.UserName,
                                                                       Upvotes = contrib.Contribution.Upvotes,
                                                                       Downvotes = contrib.Contribution.Downvotes,
                                                                       CreateDate = contrib.Contribution.CreateDate,
                                                                       GuitarOne = contrib.GuitarOne,
                                                                       GuitarTwo = contrib.GuitarTwo
                                                                   }).ToList();
            return contributionReplies;
        }

        public List<GuitarTabContributionView> GetContributionViewsByUserById(ApplicationUser user, int contributionId)
        {
            List<GuitarTabContributionView> contribution = (from contrib in _dB.GuitarTabContributions
                                                            join like in _dB.ContributionLikes on new { contributionId = contrib.ContributionId, userId = user.Id } equals
                                                                            new { contributionId = like.ContributionId, userId = like.User.Id } into contribLikes
                                                            from contribLike in contribLikes.DefaultIfEmpty()
                                                            join report in _dB.Reports on new { contributionId = (int?)contrib.ContributionId, userId = user.Id } equals
                                                                                         new { contributionId = report.ContributionId, userId = report.User.Id } into contribReports
                                                            from contribReport in contribReports.DefaultIfEmpty()
                                                            where contrib.Contribution.ContributorId == user.Id
                                                            orderby contrib.Contribution.CreateDate descending
                                                            select new GuitarTabContributionView
                                                            {
                                                                ContributionId = contrib.ContributionId,
                                                                StoryName = contrib.Contribution.Story.Title,
                                                                UserName = user.UserName,
                                                                Upvotes = contrib.Contribution.Upvotes,
                                                                Downvotes = contrib.Contribution.Downvotes,
                                                                CreateDate = contrib.Contribution.CreateDate,
                                                                GuitarOne = contrib.GuitarOne,
                                                                GuitarTwo = contrib.GuitarTwo,
                                                                Liked = contribLike != null && contribLike.UserId == user.Id && contribLike.Enabled,
                                                                LikedDate = contribLike.CreateDate,
                                                                Reported = contribReport != null && contribReport.UserId == user.Id,
                                                                ReportDate = contribReport.CreateDate
                                                            }).ToList();
            return contribution;
        }

        public List<GuitarTabContributionView> ProcessContributionModels(int currentContributionId, List<GuitarTabContributionView> guitarTabContributionReplies, int recordCount, ApplicationUser user = null)
        {
            int contributionCount = 0;
            GuitarTabContributionView bufferModel = null;
            foreach (GuitarTabContributionView viewModel in guitarTabContributionReplies)
            {
                if (contributionCount < recordCount)
                {
                    var replyCount = 1;
                    var model = GetNextHighestScoringContributionView(viewModel.ContributionId);
                    while (model != null)
                    {
                        if (replyCount > recordCount)
                        {
                            viewModel.Replies.IsAllReplies = model == null;
                            break;
                        }
                        else
                        {
                            // Add the prior contribution contents to a view model and add it to the view
                            viewModel.Replies.Contributions.Add(model);

                            // Populate with the next, highest scoring contribution replied to the previous contribution
                            model = GetNextHighestScoringContributionView(model.ContributionId, user);

                            replyCount++;
                        }
                    }
                    if (bufferModel == null)
                    {
                        viewModel.LastContributionId = currentContributionId;
                    }
                    else
                    {
                        bufferModel.NextContributionId = viewModel.ContributionId;
                        viewModel.LastContributionId = bufferModel.ContributionId;
                    }
                    bufferModel = viewModel;
                    contributionCount++;
                }
            }
            if (contributionCount != 0)
            {
                if (contributionCount > recordCount)
                {
                    guitarTabContributionReplies.RemoveAt(guitarTabContributionReplies.Count - 1);
                }
                else
                {
                    guitarTabContributionReplies.Last().IsLastContribution = true;
                }
            }
            return guitarTabContributionReplies;
        }
    }
}