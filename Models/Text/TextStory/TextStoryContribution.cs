using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{
    public class TextStoryContribution : IContribution
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int ContributionId { get; set; }
        public Contribution Contribution { get; set; }

        public int? PreviousContributionId { get; set; }
        public virtual Contribution PreviousContribution { get; set; }

        public TextStoryContribution() { }

        public TextStoryContribution(ApplicationUser user, Story story, string text)
        {
            Content = text;
            Contribution = new Contribution(user, story);
        }

        public TextStoryContribution(ApplicationUser user, Story story, TextStoryContribution previous, CreateTextStoryContributionViewModel request)
        {
            Content = request.Content;
            PreviousContribution = previous.Contribution;
            Contribution = new Contribution(user, story);
        }

        public TextStoryContribution(ApplicationUser user, Story story, Contribution previous, CreateTextStoryContributionViewModel request)
        {
            Content = request.Content;
            PreviousContribution = previous;
            Contribution = new Contribution(user, story);
        }
    }
}