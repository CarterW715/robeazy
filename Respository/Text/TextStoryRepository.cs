using RobeazyCore.Data;
using RobeazyCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobeazyCore.Respository.Text
{
    public class TextStoryRepository : IRobeazyRepository<TextStory, TextStoryViewModel, TextStoryContribution, TextStoryContributionView>
    {
        private readonly ApplicationDbContext _dB;

        public TextStoryRepository(ApplicationDbContext context)
        {
            _dB = context;
        }

        public void AddStory(TextStory story, TextStoryContribution firstContribution)
        {
            _dB.TextStories.Add(story);
            AddContribution(firstContribution); 
        }

        public TextStory GetStoryByStoryId(int storyId)
            => _dB.TextStories.FirstOrDefault(s => s.StoryId == storyId);

        public TextStoryViewModel GetStoryViewByTitle(string title, ApplicationUser user = null)
        {
            TextStoryViewModel textStory;
            if (user == null)
            {
                textStory = (from text in _dB.TextStories
                             where text.Story.Title == title
                             select new TextStoryViewModel
                             {
                                 Story = text.Story,
                                 Text = text,
                                 Liked = false,
                                 Reported = false,
                             }).FirstOrDefault();
            }
            else 
            {
                textStory = (from text in _dB.TextStories
                             join like in _dB.StoryLikes on new { storyId = text.StoryId, userId = user.Id } equals
                                                            new { storyId = like.StoryId, userId = like.UserId } into storyLikes
                             from storyLike in storyLikes.DefaultIfEmpty()
                             join report in _dB.Reports on new { storyId = (int?)text.StoryId, userId = user.Id } equals
                                                           new { storyId = report.StoryId, userId = report.UserId } into storyReports
                             from storyReport in storyReports.DefaultIfEmpty()
                             where text.Story.Title == title
                             select new TextStoryViewModel
                             {
                                 Story = text.Story,
                                 Text = text,
                                 Liked = storyLike != null && storyLike.User.Id == user.Id && storyLike.Enabled,
                                 LikedDate = storyLike.CreateDate,
                                 Reported = storyReport != null && storyReport.User.Id == user.Id,
                                 ReportDate = storyReport.CreateDate
                             }).FirstOrDefault();
            }

            return textStory;
        }

        public void AddContribution(TextStoryContribution contribution)
        {
            _dB.TextStoryContributions.Add(contribution);
            _dB.SaveChanges();
        }

        public TextStoryContribution GetContributionById(int id)
            => _dB.TextStoryContributions.Where(c => c.ContributionId == id).FirstOrDefault();

        public TextStoryContributionView GetContributionViewById(int id, ApplicationUser user = null)
        {
            TextStoryContributionView contribution;
            if (user == null)
            {
                contribution = (from contrib in _dB.TextStoryContributions
                                     where contrib.ContributionId == id
                                     select new TextStoryContributionView
                                     {
                                         ContributionId = contrib.ContributionId,
                                         PreviousContributionId = null,
                                         UserName = contrib.Contribution.Contributor.UserName,
                                         Upvotes = contrib.Contribution.Upvotes,
                                         Downvotes = contrib.Contribution.Downvotes,
                                         CreateDate = contrib.Contribution.CreateDate,
                                         Liked = false,
                                         Reported = false,
                                         Content = contrib.Content
                                     }).FirstOrDefault();
            }
            else
            {
                contribution = (from contrib in _dB.TextStoryContributions
                                     join like in _dB.ContributionLikes on new { contributionId = contrib.ContributionId, userId = user.Id } equals
                                                                          new { contributionId = like.ContributionId, userId = like.UserId } into contribLikes
                                     from contribLike in contribLikes.DefaultIfEmpty()
                                     join report in _dB.Reports on new { contributionId = (int?)contrib.ContributionId, userId = user.Id } equals
                                                                  new { contributionId = report.ContributionId, userId = report.UserId } into contribReports
                                     from contribReport in contribReports.DefaultIfEmpty()
                                     where contrib.ContributionId == id
                                     orderby contrib.Contribution.CreateDate ascending
                                     select new TextStoryContributionView
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
                                         Content = contrib.Content
                                     }).FirstOrDefault();
            }

            return contribution;
        }

        public TextStoryContributionView GetFirstContributionView(int storyId, ApplicationUser user = null)
        {
            TextStoryContributionView firstContribution;
            if (user == null)
            {
                firstContribution = (from contrib in _dB.TextStoryContributions
                                     where contrib.Contribution.StoryId == storyId
                                     orderby contrib.Contribution.CreateDate ascending
                                     select new TextStoryContributionView
                                     {
                                         ContributionId = contrib.ContributionId,
                                         PreviousContributionId = null,
                                         UserName = contrib.Contribution.Contributor.UserName,
                                         Upvotes = contrib.Contribution.Upvotes,
                                         Downvotes = contrib.Contribution.Downvotes,
                                         CreateDate = contrib.Contribution.CreateDate,
                                         Liked = false,
                                         Reported = false,
                                         Content = contrib.Content
                                     }).FirstOrDefault();
            }
            else
            {
                firstContribution = (from contrib in _dB.TextStoryContributions
                                     join like in _dB.ContributionLikes on new { contributionId = contrib.ContributionId, userId = user.Id } equals
                                                                          new { contributionId = like.ContributionId, userId = like.UserId } into contribLikes
                                     from contribLike in contribLikes.DefaultIfEmpty()
                                     join report in _dB.Reports on new { contributionId = (int?)contrib.ContributionId, userId = user.Id } equals
                                                                  new { contributionId = report.ContributionId, userId = report.UserId } into contribReports
                                     from contribReport in contribReports.DefaultIfEmpty()
                                     where contrib.Contribution.StoryId == storyId
                                     orderby contrib.Contribution.CreateDate ascending
                                     select new TextStoryContributionView
                                     {
                                         ContributionId = contrib.ContributionId,
                                         PreviousContributionId = null,
                                         UserName = contrib.Contribution.Contributor.UserName,
                                         Upvotes = contrib.Contribution.Upvotes,
                                         Downvotes = contrib.Contribution.Downvotes,
                                         CreateDate = contrib.Contribution.CreateDate,
                                         Liked = contribLike != null && contribLike.UserId == user.Id && contribLike.Enabled,
                                         LikedDate = contribLike.CreateDate,
                                         Reported = contribReport != null && contribReport.User.Id == user.Id,
                                         ReportDate = contribReport.CreateDate,
                                         Content = contrib.Content
                                     }).FirstOrDefault();
            }

            return firstContribution;
        }

        public TextStoryContributionView GetNextHighestScoringContributionView(int contributionId, ApplicationUser user = null)
        {
            TextStoryContributionView highestContribution;
            if (user == null)
            {
                highestContribution = (from contrib in _dB.TextStoryContributions
                                       where contrib.PreviousContributionId == contributionId
                                       orderby contrib.Contribution.Upvotes descending, contrib.Contribution.CreateDate ascending
                                       select new TextStoryContributionView
                                       {
                                           ContributionId = contrib.ContributionId,
                                           PreviousContributionId = contrib.PreviousContribution != null ? contrib.PreviousContributionId : (int?)null,
                                           UserName = contrib.Contribution.Contributor.UserName,
                                           Upvotes = contrib.Contribution.Upvotes,
                                           Downvotes = contrib.Contribution.Downvotes,
                                           CreateDate = contrib.Contribution.CreateDate,
                                           Liked = false,
                                           Reported = false,
                                           Content = contrib.Content
                                       }).FirstOrDefault();
            }
            else
            {
                highestContribution = (from contrib in _dB.TextStoryContributions
                                       join like in _dB.ContributionLikes on new { contributionId = contrib.ContributionId, userId = user.Id } equals
                                                                            new { contributionId = like.ContributionId, userId = like.UserId } into contribLikes
                                       from contribLike in contribLikes.DefaultIfEmpty()
                                       join report in _dB.Reports on new { contributionId = (int?)contrib.ContributionId, userId = user.Id } equals
                                                                    new { contributionId = report.ContributionId, userId = report.UserId } into contribReports
                                       from contribReport in contribReports.DefaultIfEmpty()
                                       where contrib.PreviousContributionId == contributionId
                                       orderby contrib.Contribution.Upvotes descending, contrib.Contribution.CreateDate ascending
                                       select new TextStoryContributionView
                                       {
                                           ContributionId = contrib.ContributionId,
                                           PreviousContributionId = contrib.PreviousContributionId,
                                           UserName = contrib.Contribution.Contributor.UserName,
                                           Upvotes = contrib.Contribution.Upvotes,
                                           Downvotes = contrib.Contribution.Downvotes,
                                           CreateDate = contrib.Contribution.CreateDate,
                                           Liked = contribLike != null && contribLike.UserId == user.Id && contribLike.Enabled,
                                           LikedDate = contribLike.CreateDate,
                                           Reported = contribReport != null && contribReport.UserId == user.Id,
                                           ReportDate = contribReport.CreateDate,
                                           Content = contrib.Content
                                       }).FirstOrDefault();
            }

            return highestContribution;
        }

        public List<TextStoryContributionView> GetContributionReplies(int currentContributionId, int targetContributionId, int[] idBlackList, int recordCount, bool getRelatedReplies, ApplicationUser user = null)
        {
            List<TextStoryContributionView> ContributionReplies = null;
            if (user == null)
            {
                ContributionReplies = (from contrib in _dB.TextStoryContributions
                                       where contrib.PreviousContributionId == targetContributionId
                                       && !idBlackList.Contains(contrib.ContributionId)
                                       orderby contrib.Contribution.Upvotes descending, contrib.Contribution.CreateDate ascending
                                       select new TextStoryContributionView
                                       {
                                           ContributionId = contrib.ContributionId,
                                           PreviousContributionId = contrib.ContributionId,
                                           UserName = contrib.Contribution.Contributor.UserName,
                                           Upvotes = contrib.Contribution.Upvotes,
                                           Downvotes = contrib.Contribution.Downvotes,
                                           CreateDate = contrib.Contribution.CreateDate,
                                           Liked = false,
                                           Reported = false,
                                           Content = contrib.Content
                                       }).Take(recordCount + 1).ToList();
            }
            else
            {
                ContributionReplies = (from contrib in _dB.TextStoryContributions
                                       join like in _dB.ContributionLikes on new { contributionId = contrib.ContributionId, userId = user.Id } equals
                                                                            new { contributionId = like.ContributionId, userId = like.UserId } into contribLikes
                                       from contribLike in contribLikes.DefaultIfEmpty()
                                       join report in _dB.Reports on new { contributionId = (int?)contrib.ContributionId, userId = user.Id } equals
                                                                    new { contributionId = report.ContributionId, userId = report.UserId } into contribReports
                                       from contribReport in contribReports.DefaultIfEmpty()
                                       where contrib.PreviousContributionId == targetContributionId
                                       && !idBlackList.Contains(contrib.ContributionId)
                                       orderby contrib.Contribution.Upvotes descending, contrib.Contribution.CreateDate ascending
                                       select new TextStoryContributionView
                                       {
                                           ContributionId = contrib.ContributionId,
                                           PreviousContributionId = contrib.PreviousContributionId,
                                           UserName = contrib.Contribution.Contributor.UserName,
                                           Upvotes = contrib.Contribution.Upvotes,
                                           Downvotes = contrib.Contribution.Downvotes,
                                           CreateDate = contrib.Contribution.CreateDate,
                                           Liked = contribLike != null && contribLike.UserId == user.Id && contribLike.Enabled,
                                           LikedDate = contribLike.CreateDate,
                                           Reported = contribReport != null && contribReport.UserId == user.Id,
                                           ReportDate = contribReport.CreateDate,
                                           Content = contrib.Content
                                       }).Take(recordCount + 1).ToList();
            }

            return getRelatedReplies ? ProcessContributionModels(currentContributionId, ContributionReplies, recordCount, user) : ContributionReplies;
        }

        public List<TextStoryContributionView> GetContributionViewsByUser(ApplicationUser user)
        {
            List<TextStoryContributionView> contributionReplies = (from contrib in _dB.TextStoryContributions
                                                                   where contrib.Contribution.ContributorId == user.Id
                                                                   orderby contrib.Contribution.CreateDate descending
                                                                   select new TextStoryContributionView
                                                                   {
                                                                       ContributionId = contrib.ContributionId,
                                                                       StoryName = contrib.Contribution.Story.Title,
                                                                       UserName = user.UserName,
                                                                       Upvotes = contrib.Contribution.Upvotes,
                                                                       Downvotes = contrib.Contribution.Downvotes,
                                                                       CreateDate = contrib.Contribution.CreateDate,
                                                                       Content = contrib.Content
                                                                   }).ToList();
            return contributionReplies;
        }

        public List<TextStoryContributionView> GetContributionViewsByUserById(ApplicationUser user, int contributionId)
        {
            List<TextStoryContributionView> contribution = (from contrib in _dB.TextStoryContributions
                                                            join like in _dB.ContributionLikes on new { contributionId = contrib.ContributionId, userId = user.Id } equals
                                                                            new { contributionId = like.ContributionId, userId = like.UserId } into contribLikes
                                                            from contribLike in contribLikes.DefaultIfEmpty()
                                                            join report in _dB.Reports on new { contributionId = (int?)contrib.ContributionId, userId = user.Id } equals
                                                                                         new { contributionId = report.ContributionId, userId = report.UserId } into contribReports
                                                            from contribReport in contribReports.DefaultIfEmpty()
                                                            where contrib.Contribution.ContributorId == user.Id
                                                            orderby contrib.Contribution.CreateDate descending
                                                            select new TextStoryContributionView
                                                            {
                                                                ContributionId = contrib.ContributionId,
                                                                StoryName = contrib.Contribution.Story.Title,
                                                                UserName = user.UserName,
                                                                Upvotes = contrib.Contribution.Upvotes,
                                                                Downvotes = contrib.Contribution.Downvotes,
                                                                CreateDate = contrib.Contribution.CreateDate,
                                                                Content = contrib.Content,
                                                                Liked = contribLike != null && contribLike.UserId == user.Id && contribLike.Enabled,
                                                                LikedDate = contribLike.CreateDate,
                                                                Reported = contribReport != null && contribReport.UserId == user.Id,
                                                                ReportDate = contribReport.CreateDate
                                                            }).ToList();
            return contribution;
        }

        public List<TextStoryContributionView> ProcessContributionModels(int currentContributionId, List<TextStoryContributionView> textContributionReplies, int recordCount, ApplicationUser user = null)
        {
            int contributionCount = 0;
            TextStoryContributionView bufferModel = null;
            foreach (TextStoryContributionView viewModel in textContributionReplies)
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
                    textContributionReplies.RemoveAt(textContributionReplies.Count - 1);
                }
                else
                {
                    textContributionReplies.Last().IsLastContribution = true;
                }
            }
            return textContributionReplies;
        }
    }
}