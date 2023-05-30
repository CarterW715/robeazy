using RobeazyCore.Models;
using System.Collections.Generic;

namespace RobeazyCore.Services
{
    public interface IUserService
    {
        ApplicationUser User { get; set; }
        void SetUser(string id);

        void UpdateUserAccount(EditAccountModel update);

        ViewAccountModel GetUserViewAccountModel();
        ViewAccountModel GetUserViewAccountModel(string userName);

        ContributionLike GetUserContributionLikeByUserByContribution(Contribution contribution);
        StoryLike GetUserStoryLikeByUserByStory(Story story);
        List<ContributionLikeViewModel> GetUserLikedContributions();
        List<StoryViewModel> GetUserLikedStories();

        ContributionLikeResult CreateContributionLike(int contributionId);
        ContributionLikeResult UnlikeContributionLike(int contributionId);

        StoryLikeResult CreateStoryLike(int storyId);
        StoryLikeResult UnlikeStoryLike(int storyId);

        Report GetContributionReportByUserByContribution(Contribution contribution);
        Report GetStoryReportByUserByStory(Story story);

        Report ReportContribution(ReportRequestModel request);
        Report ReportStory(ReportRequestModel request);
    }
}
