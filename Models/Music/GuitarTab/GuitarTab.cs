using RobeazyCore.Variables;
using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{
    public enum MusicGenre
    {
        None, Blues, Classical, Country, Folk, Jazz, Rock, Alternative, Grunge
    }

    public enum Key
    {
        None, A, B, C, D, E, F, G
    }

    public enum KeySignature
    {
        Natural, Sharp, Flat
    }

    public enum GuitarTuning
    {
        Standard, DropD, DropC, OpenA, OpenC, OpenD, OpenE, OpenG, StandardEFlat 
    }

    public enum PickingStyle
    {
        Pick, Fingerstyle
    }

    public class GuitarTab : IStory
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public MusicGenre Genre { get; set; }
        //Text Rules
        [Required]
        public int BarMax { get; set; }
        [Required]
        public int BarMin { get; set; }
        [Required]
        public Key Key { get; set; }
        [Required]
        public KeySignature KeySignature { get; set; }
        [Required]
        public bool IsMajor { get; set; }
        [Required]
        public GuitarTuning Tuning { get; set; }
        [Required]
        public PickingStyle PickingStyle { get; set; }

        [Required]
        public bool HasDynamicContributions { get; set; }
        public int TimeRestrictionMinutes { get; set; }

        [Required]
        public int StoryId { get; set; }
        public virtual Story Story { get; set; }

        public GuitarTab()
        {
            BarMin = GlobalVar.MUSIC_BAR_MIN;
            TimeRestrictionMinutes = GlobalVar.DEFAULT_RESTRICTION_MIN;
        }

        public GuitarTab(string Title,
                         ApplicationUser Author,
                         int Max,
                         bool IsDynamic,
                         MusicGenre SelectedGenre,
                         Key MusicKey,
                         KeySignature Signature,
                         bool IsKeyMajor)
        {
            BarMin = GlobalVar.MUSIC_BAR_MIN;
            BarMax = Max;
            HasDynamicContributions = IsDynamic;
            Genre = SelectedGenre;
            Story = new Story(Title, Author, StoryType.Music, StorySubType.GuitarTab);
        }

        public GuitarTab(CreateGuitarTabModel GuitarTab, ApplicationUser Author)
        {
            BarMin = GuitarTab.BarMin;
            BarMax = GuitarTab.BarMax;
            Tuning = GuitarTab.Tuning;
            Key = GuitarTab.Key;
            KeySignature = GuitarTab.KeySignature;
            IsMajor = GuitarTab.IsMajor;
            HasDynamicContributions = GuitarTab.DynamicContributions;
            Genre = GuitarTab.Genre;
            Story = new Story(GuitarTab.Title, Author, StoryType.Music, StorySubType.GuitarTab);
        }
    }
}