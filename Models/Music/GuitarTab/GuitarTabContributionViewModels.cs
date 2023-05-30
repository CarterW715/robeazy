using RobeazyCore.Models.Validation;
using RobeazyCore.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RobeazyCore.Models
{
    public class GuitarTabContributionView : ContributionViewModel
    {
        public string GuitarOne { get; set; }

        public string GuitarTwo { get; set; }

        public GuitarTabRepliesViewModel Replies { get; set; }

        public GuitarTabContributionView(GuitarTabContribution guitarTab)
        {
            var contribution = guitarTab.Contribution;
            ContributionId = contribution.Id;
            PreviousContributionId = guitarTab.PreviousContributionId;
            StoryName = guitarTab.Contribution.Story.Title;
            GuitarOne = guitarTab.GuitarOne;
            GuitarTwo = guitarTab.GuitarTwo;
            UserName = contribution.Contributor.UserName;
            Upvotes = contribution.Upvotes;
            Downvotes = contribution.Downvotes;
            CreateDate = contribution.CreateDate;
            Liked = false;
            Reported = false;
        }

        public GuitarTabContributionView(GuitarTabContribution guitarTab, IUserService userService)
        {
            var contribution = guitarTab.Contribution;
            ContributionId = contribution.Id;
            PreviousContributionId = guitarTab.PreviousContributionId;
            StoryName = guitarTab.Contribution.Story.Title;
            GuitarOne = guitarTab.GuitarOne;
            GuitarTwo = guitarTab.GuitarTwo;
            UserName = contribution.Contributor.UserName;
            Upvotes = contribution.Upvotes;
            Downvotes = contribution.Downvotes;
            CreateDate = contribution.CreateDate;
            var userLike = userService.GetUserContributionLikeByUserByContribution(contribution);
            Liked = userLike != null;
            var report = userService.GetContributionReportByUserByContribution(contribution);
            Reported = report != null;
        }

        public GuitarTabContributionView()
        {
            LastContributionId = null;
            NextContributionId = null;
            IsLastContribution = false;
            Replies = new GuitarTabRepliesViewModel();
        }

    }

    public class GuitarTabContributionFullViewModel : ContributionFullViewModel<GuitarTabContributionView>
    {
        public GuitarTabContributionFullViewModel(GuitarTabContribution guitarTab, int contributionDepth)
        {
            Contribution = new GuitarTabContributionView(guitarTab);
            Depth = contributionDepth;
        }

        public GuitarTabContributionFullViewModel(GuitarTabContribution guitarTab, IUserService userService, int contributionDepth)
        {
            Contribution = new GuitarTabContributionView(guitarTab, userService);
            Depth = contributionDepth;
        }

        public GuitarTabContributionFullViewModel(GuitarTabContributionView model, int contributionDepth)
        {
            Contribution = model;
            Depth = contributionDepth;
        }
    }

    public class GuitarTabRepliesViewModel
    {
        public virtual List<GuitarTabContributionView> Contributions { get; set; }
        public bool IsAllReplies { get; set; }

        public GuitarTabRepliesViewModel()
        {
            Contributions = new List<GuitarTabContributionView>();
            IsAllReplies = true;
        }
    }

    public class GuitarTabContributionReplyViewModel : ContributionReplyViewModel<GuitarTabContributionView>
    {
        public GuitarTabContributionReplyViewModel(GuitarTabContribution contribution, int depth)
        {
            Depth = depth;
            Contributions = new List<GuitarTabContributionView>(1) { new GuitarTabContributionView(contribution) };
        }

        public GuitarTabContributionReplyViewModel(List<GuitarTabContribution> contributions, int depth)
        {
            Depth = depth;
            Contributions = contributions.Select(c => new GuitarTabContributionView(c)).ToList();
        }

        public GuitarTabContributionReplyViewModel(List<GuitarTabContribution> contributions, IUserService userService, int depth)
        {
            Depth = depth;
            Contributions = contributions.Select(c => new GuitarTabContributionView(c, userService)).ToList();
        }

        public GuitarTabContributionReplyViewModel(List<GuitarTabContributionView> contributions, int depth)
        {
            Depth = depth;
            Contributions = contributions;
        }
    }

    public class CreateGuitarTabContributionViewModel : CreateContributionRequest<GuitarTab> // Redudant with CreateRequest?
    {
        [Display(Name = "Write your contribution here!")]
        public string GuitarOne { get; set; }

        public CreateGuitarTabContributionViewModel(CreateContributionRequest<GuitarTab> request)
        {
            CurrentContributionId = request.CurrentContributionId;
            TargetContributionId = request.TargetContributionId;
            Depth = request.Depth;
            StoryId = request.StoryId;
            IsContinueStory = request.IsContinueStory;
        }

        public CreateGuitarTabContributionViewModel() { }

        public override RobeazyValidationResult Validate(GuitarTab story)
        {
            return new RobeazyValidationResult(); // no validation yet :(
        }
    }
}