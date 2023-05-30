using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RobeazyCore.Models.Validation;
using RobeazyCore.Variables;

namespace RobeazyCore.Models
{

    public class CreateMovieScriptModel : CreateStoryRequest<MovieScriptContributionAggregate>
    {
        [Required]
        [Display(Name = "Script Title")]
        [StringLength(125, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Movie Genre")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MovieGenre Genre { get; set; }

        [Required]
        [Display(Name = "Maximum Elements")]
        [Range(GlobalVar.MOVIESCRIPT_ELEMENT_MAX, GlobalVar.MOVIESCRIPT_ELEMENT_MAX, ErrorMessage = "Maximum Element selection must be between {1} and {2}")]
        public int ElementMax { get; set; }

        [Display(Name = "Minimum Elements")]
        [Editable(false)]
        //[Range(3, 250, ErrorMessage = "Minimum word selection must be between {1} and {2}")]
        public int ElementMin { get; set; }

        [Required]
        [Display(Name = "Dynamic Contributions")]
        public bool DynamicContributions { get; set; } = true;

        public string ElementJson { get; set; }

        public CreateMovieScriptModel()
        {
            ElementMax = GlobalVar.MOVIESCRIPT_ELEMENT_MAX;
            ElementMin = GlobalVar.MOVIESCRIPT_ELEMENT_MIN;
            DynamicContributions = true;
        }

        public CreateMovieScriptModel(string title, MovieGenre genre, int elementMax, int elementMin, string elementJson)
        {
            Title = title;
            Genre = genre;
            ElementMax = elementMax;
            ElementMin = elementMin;
        }

        // Make sure user didn't try to make the minimum greater than the maximum
        public bool IsElementConstraintValid() => GlobalVar.MOVIESCRIPT_ELEMENT_MIN <= ElementMax;

        public override MovieScriptContributionAggregate GetFirstContribution(Story story, ApplicationUser user)
        {
            var scriptElements = JsonConvert.DeserializeObject<List<MovieScriptContribution>>(ElementJson);
            return new MovieScriptContributionAggregate(user, story, scriptElements);
        }

        public override RobeazyValidationResult Validate()
        {
            var result = new RobeazyValidationResult();

            return result;
        }
    }

    public class MovieScriptModel : StoryTypeViewModel
    {
        public virtual MovieScript Script { get; set; }
        public virtual List<MovieScriptContributionView> ContributionViewModels { get; set; }
    }

    public class MovieScriptContributionsModel
    {
        public MovieElement Element { get; set; }
        [MaxLength(1000)]
        public string Content { get; set; }
        public int Score { get; set; }
        public ApplicationUser Contributor { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class MovieScriptContributionCreate
    {
        public int Order { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MovieElement Element { get; set; }
        public string Content { get; set; }
    }

    public class MovieScriptViewModel
    {
        public int Id { get; set; }
        public string Genre { get; set; }
        public int ElementMax { get; set; }
        public int ElementMin { get; set; }
        public bool HasDynamicContributions { get; set; }
        public int ContributionTimeRestrictionMins { get; set; }
        public bool Liked { get; set; }
        public bool Reported { get; set; }
    }

}