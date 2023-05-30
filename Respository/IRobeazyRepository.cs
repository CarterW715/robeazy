using RobeazyCore.Models;
using System.Collections.Generic;

namespace RobeazyCore.Respository
{
    public interface IRobeazyRepository<StoryClass, StoryView, ContributionClass, ContributionView>
    {
        void AddStory(StoryClass story, ContributionClass firstContribution);

        StoryClass GetStoryByStoryId(int storyId);

        StoryView GetStoryViewByTitle(string title, ApplicationUser user = null);

        void AddContribution(ContributionClass contribution);

        ContributionClass GetContributionById(int id);

        ContributionView GetContributionViewById(int id, ApplicationUser user = null);
        ContributionView GetFirstContributionView(int storyId, ApplicationUser user = null);
        ContributionView GetNextHighestScoringContributionView(int contributionId, ApplicationUser user = null);
        List<ContributionView> GetContributionReplies(int CurrentContributionId, int TargetContributionId, int[] IdBlackList, int recordCount, bool GetRelatedReplies, ApplicationUser user = null);
        List<ContributionView> GetContributionViewsByUser(ApplicationUser user);
        List<ContributionView> GetContributionViewsByUserById(ApplicationUser user, int contributionId);
        List<ContributionView> ProcessContributionModels(int CurrentContributionId, List<ContributionView> ContributionReplies, int recordCount, ApplicationUser user = null);
    }
}