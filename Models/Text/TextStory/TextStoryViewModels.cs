using RobeazyCore.Models.Text.TextStory;
using RobeazyCore.Models.Validation;
using RobeazyCore.Variables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{

    public class CreateTextStoryModel : CreateStoryRequest<TextStoryContribution>
    {
        [Required]
        [Display(Name = "Story Title")]
        [StringLength(125, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Story Genre")]
        public Genre Genre { get; set; }

        [Required]
        [Display(Name = "Maximum words")]
        [Range(GlobalVar.TEXT_WORD_MIN, GlobalVar.TEXT_WORD_MAX, ErrorMessage = "Maximum word selection must be between {1} and {2}")]
        public int WordMax { get; set; }

        [Display(Name = "Minimum words")]
        [Editable(false)]
        //[Range(3, 250, ErrorMessage = "Minimum word selection must be between {1} and {2}")]
        public int WordMin { get; set; }

        [Required]
        [Display(Name = "Dynamic Contributions")]
        public bool DynamicContributions { get; set; }

        [Required]
        [Display(Name = "Make the first contribution to the story")]
        [MaxLength(10000)]
        public string FirstContribution { get; set; }

        public CreateTextStoryModel()
        {
            WordMin = GlobalVar.TEXT_WORD_MIN;
            WordMax = GlobalVar.TEXT_WORD_MAX;
            DynamicContributions = true;
        }

        // Make sure user didn't try to make the minimum greater than the maximum
        public bool IsWordConstraintValid()
        {
            return GlobalVar.TEXT_WORD_MIN <= WordMax;
        }

        public override TextStoryContribution GetFirstContribution(Story story, ApplicationUser user)
        {
            return new TextStoryContribution(user, story, FirstContribution);
        }

        public override RobeazyValidationResult Validate()
        {
            var result = new RobeazyValidationResult();

            if (Title.Length > GlobalVar.TITLE_MAX_LENGTH || Title.Length < GlobalVar.TITLE_MIN_LENGTH)
            {
                result.AddError($"The title must be between {GlobalVar.TITLE_MIN_LENGTH} and {GlobalVar.TITLE_MAX_LENGTH} words long");
            }

            if (WordMax > GlobalVar.TEXT_WORD_MAX || WordMax < GlobalVar.TEXT_WORD_MIN)
            {
                result.AddError($"Maximum word constraint must be between {GlobalVar.TEXT_WORD_MIN} and {GlobalVar.TEXT_WORD_MAX}");
            }

            if (WordMin > GlobalVar.TEXT_WORD_MAX || WordMin < GlobalVar.TEXT_WORD_MIN)
            {
                result.AddError($"Minimum word constraint must be between {GlobalVar.TEXT_WORD_MIN} and {GlobalVar.TEXT_WORD_MAX}");
            }

            if (WordMin > WordMax)
            {
                result.AddError($"Minimum word constraint must be less or equal to maximum word constraint");
            }

            var wordCountValidation = TextStoryValidation.IsContributionWordCountValid(WordMin, WordMax, FirstContribution);
            if (!wordCountValidation.Success)
            {
                result.AddError(wordCountValidation.GetErrors());
            }

            return result;
        }
    }

    public class TextStoryViewModel : StoryTypeViewModel
    {
        public virtual TextStory Text { get; set; }
        public virtual List<TextStoryContributionView> ContributionViewModels { get; set; }
        public virtual CreateTextStoryContributionViewModel SubmittedContribution { get; set; }
    }

    public class TextStoryContributionsModel
    {
        public string Content { get; set; }
        public int Score { get; set; }
        public ApplicationUser Contributor { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class CreateTextStoryViewModel
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