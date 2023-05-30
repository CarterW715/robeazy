using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{
    public class GuitarTabContribution : IContribution
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string GuitarOne { get; set; }

        public string GuitarTwo { get; set; }
        
        [Required]
        public int ContributionId { get; set; }
        public Contribution Contribution { get; set; }

        public int? PreviousContributionId { get; set; }
        public virtual Contribution PreviousContribution { get; set; }

        public GuitarTabContribution() { }

        public GuitarTabContribution(ApplicationUser user, Story story, string tab)
        {
            GuitarOne = tab;
            Contribution = new Contribution(user, story);
        }

        public GuitarTabContribution(ApplicationUser user, Story story, GuitarTabContribution previous, string tab)
        {
            GuitarOne = tab;
            PreviousContribution = previous.Contribution;
            Contribution = new Contribution(user, story);
        }

        public GuitarTabContribution(ApplicationUser user, Story story, GuitarTabContribution previous, CreateGuitarTabContributionViewModel request)
        {
            GuitarOne = request.GuitarOne;
            PreviousContribution = previous.Contribution;
            Contribution = new Contribution(user, story);
        }
    }
}