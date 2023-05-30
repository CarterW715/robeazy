using RobeazyCore.Models;

namespace RobeazyCore.Services
{
    public interface IStoryService<StoryClass, StoryView, ContributionClass, ContributionView>
        where StoryClass : IStory
        where StoryView : StoryTypeViewModel
        where ContributionClass : IContribution
        where ContributionView : ContributionViewModel
    {
        GetStoryResults<StoryView, ContributionView> GetStoryDetails(string title, ApplicationUser user, string[] contributionIds);
        CreateStoryResults<StoryClass> CreateStory(CreateStoryRequest<ContributionClass> request, ApplicationUser user);
    }
}