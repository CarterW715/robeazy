using RobeazyCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace RobeazyCore.Services
{
    public class GetInitialContributionViewsResult<ContributionView> : ServiceResult where ContributionView : ContributionViewModel
    {
        public List<ContributionView> ContributionViewModels { get; set; }

        public GetInitialContributionViewsResult(List<ContributionView> viewModels)
        {
            Success = true;
            ContributionViewModels = viewModels;
        }
    }

    public class GetRelatedContributionsResult<ContributionView> : ServiceResult where ContributionView : ContributionViewModel
    {
        public List<ContributionView> ContributionViewModels { get; set; }

        public List<int> ContributionIds { get; set; }

        public GetRelatedContributionsResult(List<ContributionView> viewModels)
        {
            Success = true;
            ContributionViewModels = viewModels;
            ContributionIds = viewModels.Select(vm => vm.ContributionId).ToList();
        }
    }

    public class ViewMoreContributionsResult<ContributionView> : ServiceResult where ContributionView : ContributionViewModel
    {
        public Queue<ContributionView> ContributionViews { get; set; }
        public Stack<int> ContributionIds { get; set; }
        public bool IsLastContribution { get; set; }
        public int LastContributionId { get; set; }

        public ViewMoreContributionsResult(Queue<ContributionView> contributionViews, Stack<int> contributionIds, bool isLastContribution, int lastContributionId)
        {
            Success = true;
            ContributionViews = contributionViews;
            ContributionIds = contributionIds;
            IsLastContribution = isLastContribution;
            LastContributionId = lastContributionId;
        }
    }

    public class CreateContributionResult<ContributionClass> : ServiceResult where ContributionClass : IContribution
    {
        public ContributionClass Contribution { get; set; }

        public CreateContributionResult()
        {
            Success = true;
        }

        public CreateContributionResult(string errorMsg)
        {
            Errors.Add(errorMsg);
        }
    }
}