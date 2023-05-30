using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{
    public enum StoryType
    {
        Text, Video, Music
    }

    public enum StorySubType
    {
        TextStory, GuitarTab, MovieScript
    }

    public class Story
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Upvotes { get; set; }
        [Required]
        public int Downvotes { get; set; }
        [Required]
        public StoryType StoryType { get; set; }
        [Required]
        public StorySubType StorySubType { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public virtual List<StoryLike> StoryLikes { get; set; }

        public virtual List<Contribution> Contributions { get; set; }

        public virtual List<TextStory> TextStories { get; set; }
        public virtual List<GuitarTab> GuitarTabs { get; set; }
        public virtual List<MovieScript> MovieScripts { get; set; }

        public virtual List<Report> Reports { get; set; }

        public Story()
        {
            Upvotes = 0;
            Downvotes = 0;
            CreateDate = DateTime.UtcNow;
        }

        public Story(string ChosenTitle, ApplicationUser StoryAuthor, StoryType Type, StorySubType SubType)
        {
            Upvotes = 0;
            Downvotes = 0;
            CreateDate = DateTime.UtcNow;
            Title = ChosenTitle;
            Author = StoryAuthor;
            StoryType = Type;
            StorySubType = SubType;
        }

        public void IncrementUpvotes()
        {
            this.Upvotes += 1;
        }

        public void DecrementUpvotes()
        {
            if (this.Upvotes > 0)
            {
                this.Upvotes -= 1;
            }
        }

    }
}
