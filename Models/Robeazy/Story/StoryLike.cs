using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{
    public class StoryLike : UserLike
    {
        [Required]
        public int StoryId { get; set; }
        public virtual Story Story { get; set; }

        public StoryLike(ApplicationUser user, Story story) : base(user)
        {
            Story = story;
        }

        public StoryLike() : base() { }
    }
}
