using RobeazyCore.Models;
using RobeazyCore.Respository;
using RobeazyCore.Variables;
using System;
using System.Collections.Generic;

namespace RobeazyCore.Services
{
    public class ContributionService<StoryClass, StoryView, ContributionClass, ContributionView> : IContributionService<StoryClass, StoryView, ContributionClass, ContributionView>
        where StoryClass : IStory
        where ContributionClass : IContribution
        where ContributionView : ContributionViewModel
    {
        private readonly IRobeazyRepository<StoryClass, StoryView, ContributionClass, ContributionView> _repo;
        private readonly INotificationService _notificationService;

        public ContributionService(IRobeazyRepository<StoryClass, StoryView, ContributionClass, ContributionView> repo, INotificationService notificationService)
        {
            _repo = repo;
            _notificationService = notificationService;
        }

        public GetInitialContributionViewsResult<ContributionView> GetInitialContributionViews(int storyId, ApplicationUser user, string[] contributionIds)
        {
            int highlightId = 0; // Id that will directed to if a user navigates directly to this contribution

            var contributionViewModels = new List<ContributionView>();

            if (contributionIds == null || contributionIds.Length == 0)
            {
                var contributionViewModel = _repo.GetFirstContributionView(storyId, user);
                if (contributionViewModel == null)
                {
                    return null;
                }

                while (contributionViewModel != null)
                {
                    // Add the prior contribution contents to a view model and add it to the view
                    contributionViewModels.Add(contributionViewModel);

                    // Populate with the next, highest scoring contribution replied to the previous contribution
                    contributionViewModel = _repo.GetNextHighestScoringContributionView(contributionViewModel.ContributionId, user);
                }
            }
            else
            {
                var id = int.Parse(contributionIds[0]);
                var contributionViewModel = _repo.GetContributionViewById(id, user);
                if (contributionViewModel == null)
                {
                    return null;
                }

                highlightId = id;
                contributionViewModels.Add(contributionViewModel);

                // Get up the chain all of the way to the root of the story
                //var previousContribution = _repo.GetContributionById(contributionViewModel.PreviousContributionId, user);// contributionViewModel.Contribution.PreviousContribution;
                var previousContributionId = contributionViewModel.PreviousContributionId;
                while (previousContributionId != null)
                {
                    var previousContributionViewModel = _repo.GetContributionViewById((int)contributionViewModel.PreviousContributionId, user);
                    contributionViewModels.Insert(0, previousContributionViewModel);
                    previousContributionId = previousContributionViewModel.PreviousContributionId; // Go up the chain
                }

                // Now add the other inputed contributions
                for (int count = 1; count < contributionIds.Length; count++)
                {
                    id = int.Parse(contributionIds[count]);
                    contributionViewModel = _repo.GetContributionViewById(id, user);
                    if (contributionViewModel == null)
                    {
                        return null;
                    }

                    contributionViewModels.Add(contributionViewModel);
                }

                // Finish out the rest of the story's contributions on this branch
                contributionViewModel = _repo.GetNextHighestScoringContributionView(contributionViewModel.ContributionId, user);

                while (contributionViewModel != null)
                {
                    contributionViewModels.Add(contributionViewModel);
                    contributionViewModel = _repo.GetNextHighestScoringContributionView(contributionViewModel.ContributionId, user);
                }
            }

            return new GetInitialContributionViewsResult<ContributionView>( contributionViewModels);
        }

        public GetRelatedContributionsResult<ContributionView> GetRelatedContributions<ContributionReplyViewModel>(RepliesRequest request, int recordCount, ApplicationUser user)
        {
            var contributions = _repo.GetContributionReplies(request.CurrentContributionId, request.PreviousContributionId, request.IdBlackList, recordCount, true, user);
            return new GetRelatedContributionsResult<ContributionView>(contributions);
        }

        public ViewMoreContributionsResult<ContributionView> ViewMoreContributions(ViewMoreRequestViewModel request, ApplicationUser user)
        {
            var model = _repo.GetNextHighestScoringContributionView(request.CurrentContributionId, user);

            int count = 1;
            bool isLastContribution = true;
            int lastContributionId = 0;
            Stack<int> contributionIds = new Stack<int>();
            Queue<ContributionView> viewModels = new Queue<ContributionView>();

            while (model != null)
            {
                contributionIds.Push(model.ContributionId);
                lastContributionId = model.ContributionId;
                if (count > GlobalVar.TEXT_CONTRIBUTION_RECORD_COUNT)
                {
                    model = _repo.GetNextHighestScoringContributionView(model.ContributionId, user);
                    isLastContribution = model == null;
                    break;
                }
                else
                {
                    // Add the prior contribution contents to a view model and add it to the view
                    //contributionHtml += RenderRazorViewToString("_ContributionPartial", new TextStoryContributionFullViewModel(model, request.Depth));
                    viewModels.Enqueue(model);
                    // Populate with the next, highest scoring contribution replied to the previous contribution
                    model = _repo.GetNextHighestScoringContributionView(model.ContributionId, user);

                    count++;
                }
            }

            return new ViewMoreContributionsResult<ContributionView>(viewModels, contributionIds, isLastContribution, lastContributionId);
        }

        public CreateContributionResult<ContributionClass> CreateContribution(CreateContributionRequest<StoryClass> request, ApplicationUser user)
        {
            var serviceResult = new CreateContributionResult<ContributionClass>();

            var story = _repo.GetStoryByStoryId(request.StoryId);
            if (story == null)
            {
                serviceResult.AddError($"The story: {request.StoryId} that is being contributed to cannot be found in the database");
                return serviceResult;
            }

            var validationResult = request.Validate(story);
            if (!validationResult.Success)
            {
                serviceResult.AddError(validationResult.GetErrors());
                return serviceResult;
            }

            var previousContribution = _repo.GetContributionById(request.TargetContributionId);
            if (previousContribution == null)
            {
                serviceResult.AddError("The contribution that is being replied to cannot be found in the database");
                return serviceResult;
            }

            var newContribution = (ContributionClass)Activator.CreateInstance(typeof(ContributionClass), user, story.Story, previousContribution, request);
            _repo.AddContribution(newContribution);
            serviceResult.Contribution = newContribution;

            _notificationService.SendContributionCreatedNotificationToAuthors(user, previousContribution.Contribution.Contributor, story.Story.Author, newContribution.Contribution, story.Story);

            return serviceResult;
        }
    }
}