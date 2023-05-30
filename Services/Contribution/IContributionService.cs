using RobeazyCore.Models;

namespace RobeazyCore.Services
{
    public interface IContributionService<StoryClass, StoryView, ContributionClass, ContributionView>
        where StoryClass : IStory
        where ContributionClass : IContribution
        where ContributionView : ContributionViewModel
    {
        GetInitialContributionViewsResult<ContributionView> GetInitialContributionViews(int storyId, ApplicationUser user, string[] contributionIds);
        GetRelatedContributionsResult<ContributionView> GetRelatedContributions<ContributionReplyViewModel>(RepliesRequest request, int recordCount, ApplicationUser user);
        ViewMoreContributionsResult<ContributionView> ViewMoreContributions(ViewMoreRequestViewModel request, ApplicationUser user);
        CreateContributionResult<ContributionClass> CreateContribution(CreateContributionRequest<StoryClass> request, ApplicationUser user);
    }
}
