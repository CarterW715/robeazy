using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RobeazyCore.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //public class ApplicationUser : IdentityUser
    //{
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
        
    //    // Email notification preferences
    //    public bool EmailOnLikedContribution { get; set; }
    //    public bool EmailOnLikedStory { get; set; }
    //    public bool EmailOnCreatedContribution { get; set; }
    //    public bool EmailOnCreatedStory { get; set; }

    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> manager)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }
    //}

    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public DbSet<Story> Stories { get; set; }
    //    public DbSet<Contribution> Contributions { get; set; }
    //    public DbSet<Feedback> Feedbacks { get; set; }\
    //    public DbSet<StoryLike> StoryLikes { get; set; }
    //    public DbSet<ContributionLike> ContributionLikes { get; set; }
    //    public DbSet<Report> Reports { get; set; }

    //    // Text Contexts
    //    public DbSet<TextStoryContribution> TextStoryContributions { get; set; }
    //    public DbSet<TextStory> TextStories { get; set; }

    //    public DbSet<MovieScriptContribution> MovieScriptContributions { get; set; }
    //    public DbSet<MovieScript> MovieScripts { get; set; }

    //    // Music 
    //    public DbSet<GuitarTabContribution> GuitarTabContributions { get; set; }
    //    public DbSet<GuitarTab> GuitarTabs { get; set; }

    //    public ApplicationDbContext()
    //        : base("DefaultConnection", throwIfV1Schema: false)
    //    {
    //    }

    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }

    //}
}