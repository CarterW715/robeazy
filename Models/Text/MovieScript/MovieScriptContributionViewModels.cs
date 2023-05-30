using RobeazyCore.Models.Validation;
using RobeazyCore.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RobeazyCore.Models
{
    public class MovieScriptContributionView : ContributionViewModel
    {
        public List<MovieScriptElementView> Elements { get; set; }
        public MovieScriptRepliesViewModel Replies { get; set; }

        public MovieScriptContributionView(List<MovieScriptContribution> contribution)
        {
            MovieScriptContribution Element = contribution.FirstOrDefault();

            ContributionId = Element.ContributionId;
            PreviousContributionId = Element.PreviousContributionId ?? 0;
            StoryName = Element.Contribution.Story.Title;
            Elements = contribution.Select(c => new MovieScriptElementView(c)).ToList();
            UserName = Element.Contribution.Contributor.UserName;
            Upvotes = Element.Contribution.Upvotes;
            Downvotes = Element.Contribution.Downvotes;
            CreateDate = Element.Contribution.CreateDate;
            Liked = false;
            Reported = false;
        }

        public MovieScriptContributionView(MovieScriptContributionAggregate contribution)
        {
            MovieScriptContribution Element = contribution.Elements.FirstOrDefault();

            ContributionId = Element.ContributionId;
            PreviousContributionId = Element.PreviousContributionId ?? 0;
            StoryName = Element.Contribution.Story.Title;
            Elements = contribution.Elements.Select(c => new MovieScriptElementView(c)).ToList();
            UserName = Element.Contribution.Contributor.UserName;
            Upvotes = Element.Contribution.Upvotes;
            Downvotes = Element.Contribution.Downvotes;
            CreateDate = Element.Contribution.CreateDate;
            Liked = false;
            Reported = false;
        }

        public MovieScriptContributionView(List<MovieScriptElementView> elements)
        {
            MovieScriptElementView element = elements.FirstOrDefault();
            ContributionId = element.Contribution_ContributionId;
            PreviousContributionId = element.PreviousContribution_ContributionId != null ? element.PreviousContribution_ContributionId : 0;
            Elements = elements;
            UserName = element.UserName;
            Upvotes = element.Upvotes;
            Downvotes = element.Downvotes;
            CreateDate = element.CreateDate;
            Liked = false;
            Reported = false;
        }

        public MovieScriptContributionView(MovieScriptContributionAggregate contribution, IUserService userService)
        {
            MovieScriptContribution Element = contribution.Elements.FirstOrDefault();

            ContributionId = Element.ContributionId;
            PreviousContributionId = Element.PreviousContributionId ?? 0;
            StoryName = Element.Contribution.Story.Title;
            Elements = contribution.Elements.Select(c => new MovieScriptElementView(c)).ToList();
            UserName = Element.Contribution.Contributor.UserName;
            Upvotes = Element.Contribution.Upvotes;
            Downvotes = Element.Contribution.Downvotes;
            CreateDate = Element.Contribution.CreateDate;
            var userLike = userService.GetUserContributionLikeByUserByContribution(contribution.Contribution);
            Liked = userLike != null;
            var report = userService.GetContributionReportByUserByContribution(contribution.Contribution);
            Reported = report != null;
        }

        public MovieScriptContributionView()
        {
            LastContributionId = null;
            NextContributionId = null;
            IsLastContribution = false;
            Replies = new MovieScriptRepliesViewModel();
        }

    }

    public class MovieScriptElementView
    {
        public int Contribution_ContributionId { get; set; }
        public int? PreviousContribution_ContributionId { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Liked { get; set; }
        public DateTime? LikedDate { get; set; }
        public bool Reported { get; set; }
        public DateTime? ReportDate { get; set; }
        public string Content { get; set; }
        public MovieElement Element { get; set; }
        public int ElementOrder { get; set; }

        public MovieScriptElementView()
        {
            Liked = false;
            Reported = false;
        }

        public MovieScriptElementView(MovieScriptContribution contribution)
        {
            Content = contribution.Content;
            Element = contribution.Element;
            ElementOrder = contribution.ElementOrder;
        }

        public static List<MovieScriptContributionView> ConvertToContributionViews(List<MovieScriptElementView> elements)
        {
            List<int> contributionIds = elements.Select(c => c.Contribution_ContributionId).Distinct().ToList();
            List<MovieScriptContributionView> contributionViewModels = new List<MovieScriptContributionView>(contributionIds.Count);
            contributionIds.ForEach(c =>
            {
                contributionViewModels.Add(new MovieScriptContributionView(elements.Where(cr => cr.Contribution_ContributionId == c).ToList()));
            });
            return contributionViewModels;
        }
    }

    public class MovieScriptContributionFullViewModel : ContributionFullViewModel<MovieScriptContributionView>
    {
        public MovieScriptContributionFullViewModel(MovieScriptContributionAggregate contribution, int contributionDepth)
        {
            Contribution = new MovieScriptContributionView(contribution);
            Depth = contributionDepth;
        }

        public MovieScriptContributionFullViewModel(MovieScriptContributionAggregate contribution, IUserService userService, int contributionDepth)
        {
            Contribution = new MovieScriptContributionView(contribution, userService);
            Depth = contributionDepth;
        }

        public MovieScriptContributionFullViewModel(MovieScriptContributionView model, int contributionDepth)
        {
            Contribution = model;
            Depth = contributionDepth;
        }
    }

    public class MovieScriptRepliesViewModel
    {
        public virtual List<MovieScriptContributionView> Contributions { get; set; }
        public bool IsAllReplies { get; set; }

        public MovieScriptRepliesViewModel()
        {
            Contributions = new List<MovieScriptContributionView>();
            IsAllReplies = true;
        }
    }

    public class MovieScriptContributionReplyViewModel : ContributionReplyViewModel<MovieScriptContributionView>
    {
        public MovieScriptContributionReplyViewModel(MovieScriptContributionAggregate contribution, int depth)
        {
            Depth = depth;
            Contributions = new List<MovieScriptContributionView>(1) { new MovieScriptContributionView(contribution) };
        }

        public MovieScriptContributionReplyViewModel(List<MovieScriptContributionAggregate> contributions, int depth)
        {
            Depth = depth;
            Contributions = contributions.Select(c => new MovieScriptContributionView(c)).ToList();
        }

        public MovieScriptContributionReplyViewModel(List<MovieScriptContributionAggregate> contributions, IUserService userService, int depth)
        {
            Depth = depth;
            Contributions = contributions.Select(c => new MovieScriptContributionView(c, userService)).ToList();
        }

        public MovieScriptContributionReplyViewModel(List<MovieScriptContributionView> contributions, int depth)
        {
            Depth = depth;
            Contributions = contributions;
        }
    }

    public class CreateMovieScriptContributionViewModel : CreateContributionRequest<MovieScript> // Redudant with CreateRequest?
    {
        [Display(Name = "Write your contribution here!")]
        public bool IsCreateScript { get; set; } = false;
        public string Content { get; set; }

        public CreateMovieScriptContributionViewModel(CreateContributionRequest<MovieScript> request)
        {
            IsCreateScript = false;
            CurrentContributionId = request.CurrentContributionId;
            TargetContributionId = request.TargetContributionId;
            Depth = request.Depth;
            StoryId = request.StoryId;
            IsContinueStory = request.IsContinueStory;
        }

        public CreateMovieScriptContributionViewModel(bool isCreateScript)
        {
            IsCreateScript = isCreateScript;
        }

        public CreateMovieScriptContributionViewModel() { }

        public override RobeazyValidationResult Validate(MovieScript story)
        {
            return new RobeazyValidationResult();
        }
    }
}