using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RobeazyCore.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        //public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ViewAccountModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailOnLikedContribution { get; set; }
        public bool EmailOnLikedStory { get; set; }
        public bool EmailOnCreatedContribution { get; set; }
        public bool EmailOnCreatedStory { get; set; }

        public List<UserContributionViewModel> MyContributions = new List<UserContributionViewModel>();
        public List<StoryViewModel> MyStories = new List<StoryViewModel>();
        public List<ContributionLikeViewModel> LikedContributions = new List<ContributionLikeViewModel>();
        public List<StoryViewModel> LikedStories = new List<StoryViewModel>();

        public ViewAccountModel() { }

        public ViewAccountModel(ApplicationUser user, List<UserContributionViewModel> contributions, List<StoryViewModel> stories, List<ContributionLikeViewModel> likedContributions, List<StoryViewModel> likedStories)
        {
            UserName = user.UserName;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            EmailOnLikedContribution = user.EmailOnLikedContribution;
            EmailOnLikedStory = user.EmailOnLikedStory;
            EmailOnCreatedContribution = user.EmailOnCreatedContribution;
            EmailOnCreatedStory = user.EmailOnCreatedStory;
            MyContributions = contributions;
            MyStories = stories;
            LikedContributions = likedContributions;
            LikedStories = likedStories;
        }
    }

    public class EditAccountModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        // Notifications
        public bool EmailOnLikedContribution { get; set; }
        public bool EmailOnLikedStory { get; set; }
        public bool EmailOnCreatedContribution { get; set; }
        public bool EmailOnCreatedStory { get; set; }
    }
}
