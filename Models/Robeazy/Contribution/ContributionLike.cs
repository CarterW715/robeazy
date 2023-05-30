using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{
    public class ContributionLike : UserLike
    {
        [Required]
        public int ContributionId { get; set; }
        public virtual Contribution Contribution { get; set; }

        public ContributionLike(ApplicationUser user, Contribution contribution) : base(user)
        {
            Contribution = contribution;
        }

        public ContributionLike() { }
    }
}
