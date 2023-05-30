using RobeazyCore.Models.Validation;
using System;

namespace RobeazyCore.Models
{
    public class StoryCompositeViewModel
    {
        public Story Story { get; set; }
        public TextStory TextStory { get; set; }
        public GuitarTab GuitarTab { get; set; }
        public MovieScript MovieScript { get; set; }
    }

    public class StoryTypeViewModel
    {
        public virtual Story Story { get; set; }
        public bool Liked { get; set; }
        public DateTime? LikedDate { get; set; }
        public bool Reported { get; set; }
        public DateTime? ReportDate { get; set; }
        public int HighlightId { get; set; }
    }

    public abstract class CreateStoryRequest<ContributionClass>
    {
        public abstract ContributionClass GetFirstContribution(Story story, ApplicationUser user);
        public abstract RobeazyValidationResult Validate();
    }

    public class StoryViewModel
    {
        public int StoryId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Liked { get; set; }
        public DateTime? LikedDate { get; set; }
        public bool Reported { get; set; }
        public DateTime? ReportDate { get; set; }
    }
}