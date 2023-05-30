using RobeazyCore.Variables;
using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{
    public enum MovieGenre
    {
        None, Horror, Adventure, SciFi, Comedy, Romance, Fantasy, Mystery, Western, Thriller
    }

    public class MovieScript : IStory
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public MovieGenre Genre { get; set; }

        [Required]
        public int ElementMax { get; set; }

        [Required]
        public int ElementMin { get; set; }

        [Required]
        public bool HasDynamicContributions { get; set; }
        public int TimeRestrictionMinutes { get; set; }

        [Required]
        public int StoryId { get; set; }
        public virtual Story Story { get; set; }

        public MovieScript()
        {
            ElementMax = GlobalVar.MOVIESCRIPT_ELEMENT_MAX;
            ElementMin = GlobalVar.MOVIESCRIPT_ELEMENT_MIN;
            TimeRestrictionMinutes = GlobalVar.DEFAULT_MOVIESCRIPT_RESTRICTION_MIN;
        }

        public MovieScript(string Title, ApplicationUser Author, int Max, bool IsDynamic, MovieGenre SelectedGenre)
        {
            ElementMin = GlobalVar.TEXT_WORD_MIN;
            ElementMax = Max;
            HasDynamicContributions = IsDynamic;
            Genre = SelectedGenre;
            Story = new Story(Title, Author, StoryType.Text, StorySubType.MovieScript);
        }
    }
}