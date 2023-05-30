using RobeazyCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobeazyCore.Services
{
    public class ReportContributionResult : ServiceResult
    {
        public Report Report { get; set; }

        public ReportContributionResult(Report report) : base()
        {
            Report = report;
        }

        public ReportContributionResult(string errorMsg) : base(errorMsg) { }
    }

    public class ContributionLikeResult : ServiceResult
    {
        public ContributionLike UserLike { get; set; }
        public Contribution Contribution { get; set; }

        public ContributionLikeResult(ContributionLike like, Contribution contribution) : base()
        {
            UserLike = like;
            Contribution = contribution;
        }

        public ContributionLikeResult(string errorMsg) : base(errorMsg) { }

        public ContributionLikeResult(bool success) : base(success) { }
    }

    public class StoryLikeResult : ServiceResult
    {
        public StoryLike UserLike { get; set; }
        public Story Story { get; set; }

        public StoryLikeResult(StoryLike like, Story story) : base()
        {
            UserLike = like;
            Story = story;
        }

        public StoryLikeResult(string errorMsg) : base(errorMsg) { }

        public StoryLikeResult(bool success) : base(success) { }
    }
}
