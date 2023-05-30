using RobeazyCore.Models.Text.TextStory;
using RobeazyCore.Models.Validation;
using RobeazyCore.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RobeazyCore.Models
{
    public class TextStoryContributionView : ContributionViewModel
    {
        public string Content { get; set; }
        public TextStoryRepliesViewModel Replies { get; set; }

        public TextStoryContributionView()
        {
            LastContributionId = null;
            NextContributionId = null;
            IsLastContribution = false;
            Replies = new TextStoryRepliesViewModel();
        }

        public TextStoryContributionView(TextStoryContribution textContribution)
        {
            ContributionId = textContribution.ContributionId;
            PreviousContributionId = textContribution.PreviousContributionId;
            StoryName = textContribution.Contribution.Story.Title;
            Content = textContribution.Content;
            UserName = textContribution.Contribution.Contributor.UserName;
            Upvotes = textContribution.Contribution.Upvotes;
            Downvotes = textContribution.Contribution.Downvotes;
            CreateDate = textContribution.Contribution.CreateDate;
            Liked = false;
            Reported = false;
        }

        public TextStoryContributionView(TextStoryContribution textContribution, IUserService userService)
        {
            var contribution = textContribution.Contribution;
            ContributionId = contribution.Id;
            PreviousContributionId = textContribution.PreviousContributionId;
            StoryName = textContribution.Contribution.Story.Title;
            Content = textContribution.Content;
            UserName = contribution.Contributor.UserName;
            Upvotes = contribution.Upvotes;
            Downvotes = contribution.Downvotes;
            CreateDate = contribution.CreateDate;
            var userLike = userService.GetUserContributionLikeByUserByContribution(contribution);
            Liked = userLike != null;
            var report = userService.GetContributionReportByUserByContribution(contribution);
            Reported = report != null;
        }
    }

    public class TextStoryContributionFullViewModel : ContributionFullViewModel<TextStoryContributionView>
    {
        public TextStoryContributionFullViewModel(TextStoryContribution contribution, int contributionDepth)
        {
            Contribution = new TextStoryContributionView(contribution);
            Depth = contributionDepth;
        }

        public TextStoryContributionFullViewModel(TextStoryContribution contribution, IUserService userService, int contributionDepth)
        {
            Contribution = new TextStoryContributionView(contribution, userService);
            Depth = contributionDepth;
        }

        public TextStoryContributionFullViewModel(TextStoryContributionView model, int contributionDepth)
        {
            Contribution = model;
            Depth = contributionDepth;
        }
    }

    public class TextStoryRepliesViewModel
    {
        public virtual List<TextStoryContributionView> Contributions { get; set; }
        public bool IsAllReplies { get; set; }

        public TextStoryRepliesViewModel()
        {
            Contributions = new List<TextStoryContributionView>();
            IsAllReplies = true;
        }
    }

    public class TextStoryContributionReplyViewModel : ContributionReplyViewModel<TextStoryContributionView>
    {
        public TextStoryContributionReplyViewModel(TextStoryContribution contribution, int depth)
        {
            Depth = depth;
            Contributions = new List<TextStoryContributionView>(1) { new TextStoryContributionView(contribution) };
        }

        public TextStoryContributionReplyViewModel(List<TextStoryContribution> contributions, int depth)
        {
            Depth = depth;
            Contributions = contributions.Select(c => new TextStoryContributionView(c)).ToList();
        }

        public TextStoryContributionReplyViewModel(List<TextStoryContribution> contributions, IUserService userService, int depth)
        {
            Depth = depth;
            Contributions = contributions.Select(c => new TextStoryContributionView(c, userService)).ToList();
        }

        public TextStoryContributionReplyViewModel(List<TextStoryContributionView> contributions, int depth)
        {
            Depth = depth;
            Contributions = contributions;
        }
    }

    public class CreateTextStoryContributionViewModel : CreateContributionRequest<TextStory> // Redudant with CreateRequest?
    {
        [Display(Name = "Write your contribution here!")]
        public string Content { get; set; }

        public CreateTextStoryContributionViewModel(CreateContributionRequest<TextStory> request)
        {
            CurrentContributionId = request.CurrentContributionId;
            TargetContributionId = request.TargetContributionId;
            Depth = request.Depth;
            StoryId = request.StoryId;
            IsContinueStory = request.IsContinueStory;
        }

        public CreateTextStoryContributionViewModel() { }

        public override RobeazyValidationResult Validate(TextStory story)
        {
            return TextStoryValidation.IsContributionWordCountValid(story.WordMin, story.WordMax, Content);
        }
    }
}