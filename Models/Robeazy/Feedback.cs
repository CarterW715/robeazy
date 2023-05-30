using System;
using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{
    public enum FeedbackType
    {
        Bug, Suggestion
    }

    public class Feedback
    {
        [Key, Required]
        public int FeedbackId { get; set; }

        [Required]
        public FeedbackType Type { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Feedback()
        {
            CreateDate = DateTime.UtcNow;
        }

        public Feedback(FeedbackType type, ApplicationUser user, string content)
        {
            Type = type;
            User = user;
            Content = content;
            CreateDate = DateTime.UtcNow;
        }
    }
}
