using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RobeazyCore.Services
{
    public class StoryServiceResults
    {
        public bool Success { get; set; }
        public List<string> Errors { get; protected set; }

        public void AddError(string errorMsg)
        {
            Success = false;
            Errors.Add(errorMsg);
        }

        public void AddErrors(List<string> errors)
        {
            Errors = Errors.Concat(errors).ToList();
        }

        public string GetErrors(string delimiter = ", ")
        {
            return string.Join(delimiter, Errors);
        }
    }

    public class GetStoryResults<StoryView, ContributionView> : StoryServiceResults
    {
        public StoryView Story { get; set; }
        public List<ContributionView> ContributionViews { get; set; }
    }

    public class CreateStoryResults<StoryClass> : StoryServiceResults
    {
        public StoryClass NewStory { get; set; }
    }
}