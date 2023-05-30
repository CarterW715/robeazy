using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{
    public class Contribution
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public int Upvotes { get; set; }

        [Required]
        public int Downvotes { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public string ContributorId { get; set; }
        public ApplicationUser Contributor { get; set; }

        [Required]
        public int StoryId { get; set; }
        public virtual Story Story { get; set; }

        public virtual List<ContributionLike> ContributionLikes { get; set; }

        public virtual List<Report> Reports { get; set; }

        //public virtual List<TextStoryContribution> TextStoryContributions { get; set; }
        //public virtual List<GuitarTabContribution> GuitarTabContributions { get; set; }
        //public virtual List<MovieScriptContribution> MovieScriptContributions { get; set; }

        public Contribution()
        {
            Upvotes = 0;
            Downvotes = 0;
            CreateDate = DateTime.UtcNow;
        }

        public Contribution(ApplicationUser user, Story story)
        {
            Upvotes = 0;
            Downvotes = 0;
            CreateDate = DateTime.UtcNow;
            Contributor = user;
            Story = story;
        }

        public void IncrementUpvotes()
        {
            Upvotes += 1;
        }

        public void DecrementUpvotes()
        {
            if (Upvotes > 0)
            {
                Upvotes -= 1;
            }
        }
    }
}
