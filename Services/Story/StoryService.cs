using RobeazyCore.Models;
using RobeazyCore.Respository;
using System;

namespace RobeazyCore.Services
{
    public class StoryService<StoryClass, StoryView, ContributionClass, ContributionView> : IStoryService<StoryClass, StoryView, ContributionClass, ContributionView>
        where StoryClass : IStory
        where StoryView : StoryTypeViewModel
        where ContributionClass : IContribution
        where ContributionView : ContributionViewModel
    {
        private readonly IContributionService<StoryClass, StoryView, ContributionClass, ContributionView> _contributionService;
        private readonly IRobeazyRepository<StoryClass, StoryView, ContributionClass, ContributionView> _repo;

        public StoryService(IContributionService<StoryClass, StoryView, ContributionClass, ContributionView> contributionService, IRobeazyRepository<StoryClass, StoryView, ContributionClass, ContributionView> repo)
        {
            _contributionService = contributionService;
            _repo = repo;
        }

        public GetStoryResults<StoryView, ContributionView> GetStoryDetails(string title, ApplicationUser user, string[] contributionIds)
        {
            var serviceResult = new GetStoryResults<StoryView, ContributionView>();

            var story = _repo.GetStoryViewByTitle(title, user);
            if (story == null)
            {
                return serviceResult;
            }
            serviceResult.Story = story;

            var contributionResult = _contributionService.GetInitialContributionViews(story.Story.Id, user, contributionIds);
            if (!contributionResult.Success)
            {
                serviceResult.AddErrors(contributionResult.Errors);
                return serviceResult;
            }

            serviceResult.ContributionViews = contributionResult.ContributionViewModels;

            return serviceResult;
        }

        public CreateStoryResults<StoryClass> CreateStory(CreateStoryRequest<ContributionClass> request, ApplicationUser user)
        {
            var serviceResult = new CreateStoryResults<StoryClass>();

            var validRequest = request.Validate();
            if (!validRequest.Success)
            {
                serviceResult.AddError(validRequest.GetErrors());
                return serviceResult;
            }

            var newStory = (StoryClass)Activator.CreateInstance(typeof(StoryClass), request, user);

            _repo.AddStory(newStory, request.GetFirstContribution(newStory.Story, user));

            serviceResult.NewStory = newStory;
            return serviceResult;
        }
    }
}