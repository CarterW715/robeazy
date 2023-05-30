using RobeazyCore.Models;
using System.Collections.Generic;

namespace RobeazyCore.Respository
{
    public interface IUserRepository
    {
        void AttachUser(ApplicationUser user);
        void SaveUserChanges();

        ApplicationUser GetUserById(string id);
        ApplicationUser GetUserByUserName(string userName);

        List<UserContributionViewModel> GetContributionsByUser(ApplicationUser user);
        ContributionLike GetUserContributionLikeByUserByContribution(ApplicationUser user, Contribution contribution);
        StoryLike GetUserStoryLikeByUserByStory(ApplicationUser user, Story story);
        List<ContributionLikeViewModel> GetUserLikedContributions(ApplicationUser user);
        List<StoryViewModel> GetUserLikedStories(ApplicationUser user);

        void AddContributionLike(ContributionLike like);
        ContributionLike EnableContributionLike(ContributionLike like);
        ContributionLike DisableContributionLike(ContributionLike like);

        void AddStoryLike(StoryLike like);
        StoryLike EnableStoryLike(StoryLike like);
        StoryLike DisableStoryLike(StoryLike like);

        Report GetContributionReportByUserByContribution(ApplicationUser user, Contribution contribution);
        Report GetStoryReportByUserByStory(ApplicationUser user, Story story);
        void AddUserReport(Report report);
    }
}
