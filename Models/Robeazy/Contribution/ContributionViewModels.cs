using RobeazyCore.Models.Validation;
using System;
using System.Collections.Generic;

namespace RobeazyCore.Models
{
    public abstract class ContributionViewModel
    {
        public int ContributionId { get; set; }
        public int? PreviousContributionId { get; set; }
        public int? LastContributionId { get; set; }
        public int? NextContributionId { get; set; }
        public bool IsLastContribution { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public string UserName { get; set; }
        public string StoryName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Liked { get; set; }
        public DateTime? LikedDate { get; set; }
        public bool Reported { get; set; }
        public DateTime? ReportDate { get; set; }

        public ContributionViewModel() { }
    }

    public class UserContributionViewModel
    {
        public int ContributionId { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public string UserName { get; set; }
        public string StoryName { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class ContributionLikeViewModel : UserContributionViewModel
    {
        public bool Liked { get; set; }
        public DateTime? LikedDate { get; set; }
    }

    public class ContributionReportViewModel : UserContributionViewModel
    {
        public bool Reported { get; set; }
        public DateTime? ReportDate { get; set; }
    }

    public class ContributionFullViewModel<ContributionView>
    {
        public ContributionView Contribution { get; set; }
        public int Depth { get; set; }
    }

    public class ContributionReplyViewModel<ContributionView>
    {
        public int Depth { get; set; }
        public List<ContributionView> Contributions { get; set; }
    }

    public class ViewMoreRequestViewModel
    {
        public int CurrentContributionId { get; set; }
        public int ContributionCount { get; set; }
        public int Depth { get; set; }
    }

    public abstract class CreateContributionRequest<StoryClass> where StoryClass : IStory
    {
        public int CurrentContributionId { get; set; }
        public int TargetContributionId { get; set; }
        public int? LastContributionId { get; set; }
        public int Depth { get; set; }
        public int StoryId { get; set; }
        public bool IsContinueStory { get; set; }
        public StorySubType SubType { get; set; }

        public abstract RobeazyValidationResult Validate(StoryClass story);
    }

    public class RepliesRequest
    {
        public int CurrentContributionId { get; set; }
        public int PreviousContributionId { get; set; }
        public int Depth { get; set; }
        public int[] IdBlackList { get; set; }
    }
}