using RobeazyCore.Variables;
using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{
    public enum Genre
    {
        None, Horror, Adventure, SciFi, Comedy, Romance, Fantasy, Mystery, Western, Thriller, FanFiction
    }

    public class TextStory : IStory
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        public int WordMax { get; set; }

        [Required]
        public int WordMin { get; set; }

        [Required]
        public bool HasDynamicContributions { get; set; }
        public int TimeRestrictionMinutes { get; set; }

        [Required]
        public int StoryId { get; set; }
        public virtual Story Story { get; set; }

        public TextStory()
        {
            WordMin = GlobalVar.TEXT_WORD_MIN;
            TimeRestrictionMinutes = GlobalVar.DEFAULT_RESTRICTION_MIN;
        }

        public TextStory(string Title, ApplicationUser Author, int Max, bool IsDynamic, Genre SelectedGenre)
        {
            WordMin = GlobalVar.TEXT_WORD_MIN;           
            WordMax = Max;
            HasDynamicContributions = IsDynamic;
            Genre = SelectedGenre;
            Story = new Story(Title, Author, StoryType.Text, StorySubType.TextStory);
        }

        public TextStory(CreateTextStoryModel request, ApplicationUser Author)
        {
            WordMin = request.WordMin;
            WordMax = request.WordMax;
            HasDynamicContributions = request.DynamicContributions;
            Genre = request.Genre;
            Story = new Story(request.Title, Author, StoryType.Text, StorySubType.TextStory);
        }
    }
}