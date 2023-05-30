using System;
using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{
    public class Report
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int? StoryId { get; set; }
        public Story Story { get; set; }

        public int? ContributionId { get; set; }
        public Contribution Contribution { get; set; }

        public Report(ApplicationUser user, Contribution contribution, string reason)
        {
            User = user;
            Contribution = contribution;
            Reason = reason;
            CreateDate = DateTime.UtcNow;
        }

        public Report(ApplicationUser user, Story story, string reason)
        {
            User = user;
            Story = story;
            Reason = reason;
            CreateDate = DateTime.UtcNow;
        }

        public Report() { }
    }
}
