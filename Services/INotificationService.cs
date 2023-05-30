using RobeazyCore.Models;

namespace RobeazyCore.Services
{
    public interface INotificationService
    {
        void SendContributionCreatedNotificationToAuthors(ApplicationUser creator, ApplicationUser previousContributionAuthor, ApplicationUser storyAuthor, Contribution newContribution, Story story);
    }
}
