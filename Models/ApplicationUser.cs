using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobeazyCore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual List<Story> Stories { get; set; }
        public virtual List<StoryLike> StoryLikes { get; set; }

        public virtual List<Contribution> Contributions { get; set; }
        public virtual List<ContributionLike> ContributionLikes { get; set; }

        public virtual List<Feedback> Feedbacks { get; set; }
        public virtual List<Report> Reports { get; set; }

        // Email notification preferences
        public bool EmailOnLikedContribution { get; set; }
        public bool EmailOnLikedStory { get; set; }
        public bool EmailOnCreatedContribution { get; set; }
        public bool EmailOnCreatedStory { get; set; }
    }
}
