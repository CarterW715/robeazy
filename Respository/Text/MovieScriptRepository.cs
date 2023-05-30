using RobeazyCore.Data;
using RobeazyCore.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Data.SqlClient;

namespace RobeazyCore.Respository.Text
{
    public class MovieScriptRepository : IRobeazyRepository<MovieScript, MovieScriptModel, MovieScriptContributionAggregate, MovieScriptContributionView>
    {
        private readonly ApplicationDbContext _dB;

        public MovieScriptRepository(ApplicationDbContext context)
        {
            _dB = context;
        }

        public void AddStory(MovieScript story, MovieScriptContributionAggregate firstContribution)
        {
            _dB.MovieScripts.Add(story);
            AddContribution(firstContribution);
        }

        public MovieScript GetStoryByStoryId(int storyId)
            => _dB.MovieScripts.FirstOrDefault(s => s.StoryId == storyId);

        public MovieScriptModel GetStoryViewByTitle(string title, ApplicationUser user = null)
        {
            MovieScriptModel movieScript;
            if (user == null)
            {
                movieScript = (from script in _dB.MovieScripts
                               where script.Story.Title == title
                               select new MovieScriptModel
                               {
                                   Story = script.Story,
                                   Script = script,
                                   Liked = false,
                                   Reported = false,
                               }).FirstOrDefault();
            }
            else
            {
                movieScript = (from script in _dB.MovieScripts
                               join like in _dB.StoryLikes on new { storyId = script.StoryId, userId = user.Id } equals
                                                              new { storyId = like.StoryId, userId = like.UserId } into storyLikes
                               from storyLike in storyLikes.DefaultIfEmpty()
                               join report in _dB.Reports on new { storyId = (int?)script.StoryId, userId = user.Id } equals
                                                             new { storyId = report.StoryId, userId = report.UserId } into storyReports
                               from storyReport in storyReports.DefaultIfEmpty()
                               where script.Story.Title == title
                               select new MovieScriptModel
                               {
                                   Story = script.Story,
                                   Script = script,
                                   Liked = storyLike != null && storyLike.UserId == user.Id && storyLike.Enabled,
                                   LikedDate = storyLike.CreateDate,
                                   Reported = storyReport != null && storyReport.UserId == user.Id,
                                   ReportDate = storyReport.CreateDate
                               }).FirstOrDefault();
            }

            return movieScript;
        }

        public void AddContribution(MovieScriptContributionAggregate contribution)
        {
            _dB.MovieScriptContributions.AddRange(contribution.Elements);
            _dB.SaveChanges();
        }

        public MovieScriptContributionAggregate GetContributionById(int id)
            => new MovieScriptContributionAggregate(_dB.MovieScriptContributions.Where(c => c.ContributionId == id).ToList());

        public MovieScriptContributionView GetContributionViewById(int id, ApplicationUser user = null)
        {
            List<MovieScriptElementView> contribution = null;
            if (user == null)
            {
                contribution = (from contrib in _dB.MovieScriptContributions
                                where contrib.ContributionId == id
                                select new MovieScriptElementView
                                {
                                    Contribution_ContributionId = contrib.ContributionId,
                                    PreviousContribution_ContributionId = contrib.PreviousContributionId,
                                    UserName = contrib.Contribution.Contributor.UserName,
                                    Upvotes = contrib.Contribution.Upvotes,
                                    Downvotes = contrib.Contribution.Downvotes,
                                    CreateDate = contrib.Contribution.CreateDate,
                                    Liked = false,
                                    Reported = false,
                                    Content = contrib.Content,
                                    ElementOrder = contrib.ElementOrder,
                                    Element = contrib.Element
                                }).ToList();
            }
            else
            {
                contribution = (from contrib in _dB.MovieScriptContributions
                                join like in _dB.ContributionLikes on new { contributionId = contrib.ContributionId, userId = user.Id } equals
                                                                      new { contributionId = like.ContributionId, userId = like.UserId } into contribLikes
                                from contribLike in contribLikes.DefaultIfEmpty()
                                join report in _dB.Reports on new { contributionId = (int?)contrib.ContributionId, userId = user.Id } equals
                                                              new { contributionId = report.ContributionId, userId = report.UserId } into contribReports
                                from contribReport in contribReports.DefaultIfEmpty()
                                where contrib.ContributionId == id
                                orderby contrib.Contribution.CreateDate ascending
                                select new MovieScriptElementView
                                {
                                    Contribution_ContributionId = contrib.ContributionId,
                                    PreviousContribution_ContributionId = contrib.PreviousContributionId,
                                    UserName = contrib.Contribution.Contributor.UserName,
                                    Upvotes = contrib.Contribution.Upvotes,
                                    Downvotes = contrib.Contribution.Downvotes,
                                    CreateDate = contrib.Contribution.CreateDate,
                                    Liked = contribLike != null && contribLike.UserId == user.Id && contribLike.Enabled,
                                    LikedDate = contribLike.CreateDate,
                                    Reported = contribReport != null && contribReport.UserId == user.Id,
                                    ReportDate = contribReport.CreateDate,
                                    Content = contrib.Content,
                                    ElementOrder = contrib.ElementOrder,
                                    Element = contrib.Element
                                }).ToList();
            }

            return new MovieScriptContributionView(contribution);
        }

        public MovieScriptContributionView GetFirstContributionView(int storyId, ApplicationUser user = null)
        {
            List<MovieScriptElementView> firstContribution = null;
            if (user == null)
            {
                firstContribution = (from contrib in _dB.MovieScriptContributions
                                     where contrib.Contribution.StoryId == storyId
                                     && contrib.PreviousContributionId == null
                                     orderby contrib.ElementOrder ascending
                                     select new MovieScriptElementView
                                     {
                                         Contribution_ContributionId = contrib.ContributionId,
                                         PreviousContribution_ContributionId = contrib.PreviousContributionId,
                                         UserName = contrib.Contribution.Contributor.UserName,
                                         Upvotes = contrib.Contribution.Upvotes,
                                         Downvotes = contrib.Contribution.Downvotes,
                                         CreateDate = contrib.Contribution.CreateDate,
                                         Liked = false,
                                         Reported = false,
                                         Content = contrib.Content,
                                         ElementOrder = contrib.ElementOrder,
                                         Element = contrib.Element
                                     }).ToList();
            }
            else
            {
                firstContribution = (from contrib in _dB.MovieScriptContributions
                                     join like in _dB.ContributionLikes on new { contributionId = contrib.ContributionId, userId = user.Id } equals
                                                                          new { contributionId = like.ContributionId, userId = like.UserId } into contribLikes
                                     from contribLike in contribLikes.DefaultIfEmpty()
                                     join report in _dB.Reports on new { contributionId = (int?)contrib.ContributionId, userId = user.Id } equals
                                                                  new { contributionId = report.ContributionId, userId = report.UserId } into contribReports
                                     from contribReport in contribReports.DefaultIfEmpty()
                                     where contrib.Contribution.StoryId == storyId
                                     && contrib.PreviousContributionId == null
                                     orderby contrib.ElementOrder ascending
                                     select new MovieScriptElementView
                                     {
                                         Contribution_ContributionId = contrib.ContributionId,
                                         PreviousContribution_ContributionId = contrib.PreviousContributionId,
                                         UserName = contrib.Contribution.Contributor.UserName,
                                         Upvotes = contrib.Contribution.Upvotes,
                                         Downvotes = contrib.Contribution.Downvotes,
                                         CreateDate = contrib.Contribution.CreateDate,
                                         Liked = contribLike != null && contribLike.UserId == user.Id && contribLike.Enabled,
                                         LikedDate = contribLike.CreateDate,
                                         Reported = contribReport != null && contribReport.UserId == user.Id,
                                         ReportDate = contribReport.CreateDate,
                                         Content = contrib.Content,
                                         ElementOrder = contrib.ElementOrder,
                                         Element = contrib.Element
                                     }).ToList();
            }

            return new MovieScriptContributionView(firstContribution);
        }

        public MovieScriptContributionView GetNextHighestScoringContributionView(int targetContributionId, ApplicationUser user = null)
        {
            List<MovieScriptElementView> highestContribution;
            if (user == null)
            {
                highestContribution = _dB.MovieScriptContributions.FromSqlRaw(@"select * from MovieScriptContributions m
                                                                                        join Contributions c on c.Id = m.Contribution_Id
                                                                                        join (select Id, UserName from AspNetUsers) u on u.Id = c.Contributor_Id
                                                                                        join(select TOP 1 m2.Contribution_Id, c.Upvotes, c.CreateDate from MovieScriptContributions m2
                                                                                        join Contributions c on c.ContributionId = m2.Contribution_ContributionId
                                                                                        where m2.PreviousContribution_ContributionId = @targetContributionId
                                                                                        group by m2.Contribution_ContributionId, c.Upvotes, c.CreateDate
                                                                                        order by c.Upvotes DESC, c.CreateDate ASC) sub on sub.Contribution_ContributionId = m.Contribution_ContributionId
                                                                                        order by m.ElementOrder",
                                                                                        new SqlParameter("targetContributionId", targetContributionId)).Select(c => new MovieScriptElementView(c)).ToList();
            }
            else
            {
                highestContribution = _dB.MovieScriptContributions.FromSqlRaw(@"select * from MovieScriptContributions m
                                                                                        join Contributions c on c.ContributionId = m.Contribution_ContributionId
                                                                                        left join (select Contribution_ContributionId, CreateDate as LikeDate from ContributionLikes where Enabled = 1) l on l.Contribution_ContributionId = m.Contribution_ContributionId
                                                                                        left join (select Contribution_ContributionId, CreateDate as ReportDate from Reports) r on r.Contribution_ContributionId = m.Contribution_ContributionId
                                                                                        join (select Id, UserName from AspNetUsers) u on u.Id = c.Contributor_Id
                                                                                        join(select TOP 1 m2.Contribution_ContributionId, c.Upvotes, c.CreateDate from MovieScriptContributions m2
                                                                                        join Contributions c on c.ContributionId = m2.Contribution_ContributionId
                                                                                        where m2.PreviousContribution_ContributionId = @targetContributionId
                                                                                        group by m2.Contribution_ContributionId, c.Upvotes, c.CreateDate
                                                                                        order by c.Upvotes DESC, c.CreateDate ASC) sub on sub.Contribution_ContributionId = m.Contribution_ContributionId
                                                                                        order by m.ElementOrder",
                                                                                        new SqlParameter("targetContributionId", targetContributionId)).Select(c => new MovieScriptElementView(c)).ToList();
            }

            return new MovieScriptContributionView(highestContribution);
        }

        public List<MovieScriptContributionView> GetContributionReplies(int currentContributionId, int targetContributionId, int[] idBlackList, int recordCount, bool getRelatedReplies, ApplicationUser user = null)
        {
            List<string> parameterStrings = new List<string>(idBlackList.Count());
            List<SqlParameter> parameters = new List<SqlParameter>(idBlackList.Count());
            foreach (int id in idBlackList)
            {
                string parameterName = $"@id{id}";
                parameterStrings.Add(parameterName);
                parameters.Add(new SqlParameter(parameterName, id));
            }
            parameters.Add(new SqlParameter("count", recordCount + 1));
            parameters.Add(new SqlParameter("idBlackList", string.Join(",", idBlackList)));
            parameters.Add(new SqlParameter("targetContributionId", targetContributionId));

            List<MovieScriptElementView> contributionReplies;
            if (user == null)
            {
                contributionReplies = _dB.MovieScriptContributions.FromSqlRaw($@"select * from MovieScriptContributions m
                                                                                         join Contributions c on c.ContributionId = m.Contribution_ContributionId                                                                                        
                                                                                         join (select Id, UserName from AspNetUsers) u on u.Id = c.Contributor_Id
                                                                                         join (select TOP (@count) m2.Contribution_ContributionId, c.Upvotes, c.CreateDate from MovieScriptContributions m2
                                                                                         join Contributions c on c.ContributionId = m2.Contribution_ContributionId
                                                                                         where m2.PreviousContribution_ContributionId = @targetContributionId and
                                                                                         m2.Contribution_ContributionId not in ({string.Join(",", parameterStrings)})
                                                                                         group by m2.Contribution_ContributionId, c.Upvotes, c.CreateDate
                                                                                         order by c.Upvotes DESC, c.CreateDate ASC) sub on sub.Contribution_ContributionId = m.Contribution_ContributionId
                                                                                         order by m.Contribution_ContributionId, sub.Upvotes, sub.CreateDate ASC, m.ElementOrder",
                                                                                         parameters.ToArray()).Select(c => new MovieScriptElementView(c)).ToList();
            }
            else
            {
                contributionReplies = _dB.MovieScriptContributions.FromSqlRaw($@"select * from MovieScriptContributions m
                                                                                        join Contributions c on c.ContributionId = m.Contribution_ContributionId
                                                                                        left join (select Contribution_ContributionId, CreateDate as LikeDate from ContributionLikes where Enabled = 1) l on l.Contribution_ContributionId = m.Contribution_ContributionId
                                                                                        left join (select Contribution_ContributionId, CreateDate as ReportDate from Reports) r on r.Contribution_ContributionId = m.Contribution_ContributionId
                                                                                        join (select Id, UserName from AspNetUsers) u on u.Id = c.Contributor_Id
                                                                                        join (select TOP (@count) m2.Contribution_ContributionId, c.Upvotes, c.CreateDate from MovieScriptContributions m2
                                                                                        join Contributions c on c.ContributionId = m2.Contribution_ContributionId
                                                                                        where m2.PreviousContribution_ContributionId = @targetContributionId and
                                                                                        m2.Contribution_ContributionId not in ({string.Join(", ", parameterStrings)})
                                                                                        group by m2.Contribution_ContributionId, c.Upvotes, c.CreateDate
                                                                                        order by c.Upvotes DESC, c.CreateDate ASC) sub on sub.Contribution_ContributionId = m.Contribution_ContributionId
                                                                                        order by m.Contribution_ContributionId, sub.Upvotes, sub.CreateDate ASC, m.ElementOrder",
                                                                                        parameters.ToArray()).Select(c => new MovieScriptElementView(c)).ToList();            }

            List<MovieScriptContributionView> contributionViewModels = MovieScriptElementView.ConvertToContributionViews(contributionReplies);
            return getRelatedReplies ? ProcessContributionModels(currentContributionId, contributionViewModels, recordCount, user) : contributionViewModels;
        }

        public List<MovieScriptContributionView> GetContributionViewsByUser(ApplicationUser user)
        {
            List<MovieScriptElementView> contributionElements = (from contrib in _dB.MovieScriptContributions
                                                                 where contrib.Contribution.ContributorId == user.Id
                                                                 orderby contrib.Contribution.CreateDate descending
                                                                 select new MovieScriptElementView
                                                                 {
                                                                     Contribution_ContributionId = contrib.ContributionId,
                                                                     PreviousContribution_ContributionId = contrib.PreviousContributionId,
                                                                     UserName = contrib.Contribution.Contributor.UserName,
                                                                     Upvotes = contrib.Contribution.Upvotes,
                                                                     Downvotes = contrib.Contribution.Downvotes,
                                                                     CreateDate = contrib.Contribution.CreateDate,
                                                                     Content = contrib.Content,
                                                                     ElementOrder = contrib.ElementOrder,
                                                                     Element = contrib.Element
                                                                 }).ToList();

            var contributionIds = contributionElements.Select(c => c.Contribution_ContributionId).Distinct().ToList();

            var contributionViews = new List<MovieScriptContributionView>(contributionIds.Count);
            foreach (var contributionId in contributionIds)
            {
                contributionViews.Add(new MovieScriptContributionView(contributionElements.Where(e => e.Contribution_ContributionId == contributionId).ToList()));
            }

            return contributionViews;
        }

        public List<MovieScriptContributionView> GetContributionViewsByUserById(ApplicationUser user, int contributionId)
        {
            List<MovieScriptElementView> contribution = (from contrib in _dB.MovieScriptContributions
                                                         join like in _dB.ContributionLikes on new { contributionId = contrib.ContributionId, userId = user.Id } equals
                                                                         new { contributionId = like.ContributionId, userId = like.UserId } into contribLikes
                                                         from contribLike in contribLikes.DefaultIfEmpty()
                                                         join report in _dB.Reports on new { contributionId = (int?)contrib.ContributionId, userId = user.Id } equals
                                                                                      new { contributionId = report.ContributionId, userId = report.UserId } into contribReports
                                                         from contribReport in contribReports.DefaultIfEmpty()
                                                         where contrib.Contribution.ContributorId == user.Id
                                                         orderby contrib.Contribution.CreateDate descending
                                                         select new MovieScriptElementView
                                                         {
                                                             Contribution_ContributionId = contrib.ContributionId,
                                                             PreviousContribution_ContributionId = contrib.PreviousContributionId,
                                                             UserName = contrib.Contribution.Contributor.UserName,
                                                             Upvotes = contrib.Contribution.Upvotes,
                                                             Downvotes = contrib.Contribution.Downvotes,
                                                             CreateDate = contrib.Contribution.CreateDate,
                                                             Content = contrib.Content,
                                                             ElementOrder = contrib.ElementOrder,
                                                             Element = contrib.Element,
                                                             Liked = contribLike != null && contribLike.UserId == user.Id && contribLike.Enabled,
                                                             LikedDate = contribLike.CreateDate,
                                                             Reported = contribReport != null && contribReport.UserId == user.Id,
                                                             ReportDate = contribReport.CreateDate
                                                         }).ToList();

            return new List<MovieScriptContributionView>() { new MovieScriptContributionView(contribution) };
        }

        public List<MovieScriptContributionView> ProcessContributionModels(int currentContributionId, List<MovieScriptContributionView> contributionReplies, int recordCount, ApplicationUser user = null)
        {
            int contributionCount = 0;
            MovieScriptContributionView bufferModel = null;
            foreach (MovieScriptContributionView viewModel in contributionReplies)
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
                    contributionReplies.RemoveAt(contributionReplies.Count - 1);
                }
                else
                {
                    contributionReplies.Last().IsLastContribution = true;
                }
            }
            return contributionReplies;
        }
    }
}