using RobeazyCore.Models.Validation;
using RobeazyCore.Variables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{

    public class CreateGuitarTabModel : CreateStoryRequest<GuitarTabContribution>
    {
        [Required]
        [Display(Name = "Title")]
        [StringLength(125, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Music Genre")]
        public MusicGenre Genre { get; set; }

        [Required]
        [Display(Name = "Tuning")]
        public GuitarTuning Tuning { get; set; }

        [Required]
        [Display(Name = "Key")]
        public Key Key { get; set; }

        [Display(Name = "Key Signature")]
        public KeySignature KeySignature { get; set; }

        [Display(Name = "Tonality")]
        public bool IsMajor { get; set; }

        [Display(Name = "Picking Style")]
        public PickingStyle PickingStyle { get; set; }

        [Required]
        [Display(Name = "Maximum bars")]
        [Range(GlobalVar.MUSIC_BAR_MIN, GlobalVar.MUSIC_BAR_MAX, ErrorMessage = "Maximum bar selection must be between {1} and {2}")]
        public int BarMax { get; set; }

        [Display(Name = "Minimum bars")]
        [Editable(false)]
        //[Range(3, 250, ErrorMessage = "Minimum word selection must be between {1} and {2}")]
        public int BarMin { get; set; }

        [Required]
        [Display(Name = "Dynamic Contributions")]
        public bool DynamicContributions { get; set; }

        [Required]
        [Display(Name = "Make the first contribution to the song")]
        [MaxLength(10000)]
        public string GuitarOne { get; set; }

        public CreateGuitarTabModel()
        {
            IsMajor = true;
            Tuning = GuitarTuning.Standard;
            Key = Key.None;
            BarMin = GlobalVar.MUSIC_BAR_MIN;
            BarMax = GlobalVar.MUSIC_BAR_MAX;
            DynamicContributions = true;
        }

        public override GuitarTabContribution GetFirstContribution(Story story, ApplicationUser user)
        {
            return new GuitarTabContribution(user, story, GuitarOne);
        }

        public override RobeazyValidationResult Validate()
        {
            var result = new RobeazyValidationResult();

            if (Title.Length > GlobalVar.TITLE_MAX_LENGTH || Title.Length < GlobalVar.TITLE_MIN_LENGTH)
            {
                result.AddError($"The title must be between {GlobalVar.TITLE_MIN_LENGTH} and {GlobalVar.TITLE_MAX_LENGTH} words long");
            }

            if (BarMax > GlobalVar.MUSIC_BAR_MAX || BarMax < GlobalVar.MUSIC_BAR_MIN)
            {
                result.AddError($"Maximum bar constraint must be between {GlobalVar.MUSIC_BAR_MIN} and {GlobalVar.MUSIC_BAR_MAX}");
            }

            if (BarMin > GlobalVar.MUSIC_BAR_MAX || BarMin < GlobalVar.MUSIC_BAR_MIN)
            {
                result.AddError($"Minimum bar constraint must be between {GlobalVar.MUSIC_BAR_MIN} and {GlobalVar.MUSIC_BAR_MAX}");
            }

            if (BarMin > BarMax)
            {
                result.AddError($"Minimum bar constraint must be less or equal to maximum bar constraint");
            }

            return result;
        }
    }

    public class GuitarTabViewModel : StoryTypeViewModel
    {
        public virtual GuitarTab Tab { get; set; }
        public virtual List<GuitarTabContributionView> ContributionViewModels { get; set; }
        public virtual CreateGuitarTabContributionViewModel SubmittedContribution { get; set; }
    }

    public class GuitarTabContributionsModel
    {
        public string Content { get; set; }
        public int Score { get; set; }
        public ApplicationUser Contributor { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class GuiatTabViewModel
    {
        public int Id { get; set; }
        public string Genre { get; set; }
        public int WordMax { get; set; }
        public int WordMin { get; set; }
        public bool HasDynamicContributions { get; set; }
        public int ContributionTimeRestrictionMins { get; set; }
        public bool Liked { get; set; }
        public bool Reported { get; set; }
    }
}