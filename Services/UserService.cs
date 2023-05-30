using RobeazyCore.Models;
using RobeazyCore.Respository;
using System.Collections.Generic;

namespace RobeazyCore.Services
{
    public class UserService : IUserService
	{
        public ApplicationUser User { get; set; }

        private readonly IUserRepository _userRepo;
        private readonly IStoryRepository _storyRepo;
        private readonly IContributionRepository _contributionRepo;

        public UserService(IUserRepository repo, IStoryRepository storyRepo, IContributionRepository contributionRepo)
        {
            _userRepo = repo;
            _storyRepo = storyRepo;
            _contributionRepo = contributionRepo;
        }

        public void SetUser(string id)
        {
            User = _userRepo.GetUserById(id);
        }

        public ViewAccountModel GetUserViewAccountModel()
        {
            if (User == null)
            {
                return null;
            }

            var contributions = _userRepo.GetContributionsByUser(User);
            var stories = _storyRepo.GetStoryViewModelsByUser(User);
            var likedContributions = GetUserLikedContributions(User);
            var likedStories = GetUserLikedStories(User);

            return new ViewAccountModel(User, contributions, stories, likedContributions, likedStories);
        }

        public ViewAccountModel GetUserViewAccountModel(string userName)
        {
            var user = _userRepo.GetUserByUserName(userName);
            if (user == null)
            {
                return null;
            }

            var contributions = _userRepo.GetContributionsByUser(user);
            var stories = _storyRepo.GetStoryViewModelsByUser(user);
            var likedContributions = GetUserLikedContributions(user);
            var likedStories = GetUserLikedStories(user);

            return new ViewAccountModel(user, contributions, stories, likedContributions, likedStories);
        }

        public void UpdateUserAccount(EditAccountModel update)
        {
            _userRepo.AttachUser(User);

            User.FirstName = update.FirstName;
            User.LastName = update.LastName;
            User.Email = update.Email;
            User.EmailOnCreatedContribution = update.EmailOnCreatedContribution;
            User.EmailOnCreatedStory = update.EmailOnCreatedStory;
            User.EmailOnLikedContribution = update.EmailOnLikedContribution;
            User.EmailOnLikedStory = update.EmailOnLikedStory;

            _userRepo.SaveUserChanges();
        }

        public Report ReportContribution(ReportRequestModel request)
        {
            var contribution = _contributionRepo.GetContributionById(request.EntityId);
            if (contribution == null)
            {
                return null;
            }
            var userReport = _userRepo.GetContributionReportByUserByContribution(User, contribution);
            if (userReport == null)
            {
                return null;
            }
            var report = new Report(User, contribution, request.Reason);
            _userRepo.AddUserReport(report);
            return report;
        }

        public Report ReportStory(ReportRequestModel request)
        {
            var story = _storyRepo.GetStoryById(request.EntityId);
            if (story == null)
            {
                return null;
            }
            var userReport = _userRepo.GetStoryReportByUserByStory(User, story);
            if (userReport == null)
            {
                return null;
            }
            var report = new Report(User, story, request.Reason);
            _userRepo.AddUserReport(report);
            return report;
        }

        public ContributionLikeResult CreateContributionLike(int contributionId)
        {
            var contribution = _contributionRepo.GetContributionById(contributionId);
            if (contribution == null)
            {
                return null;
            }
            var contributionAuthor = contribution.Contributor;
            if (User.Id == contributionAuthor.Id)
            {
                return null; // cant like your own contribution
            }

            bool allowNotification = true; // Prevent like -> unlike -> like email spam
            var userLike = GetUserContributionLikeByUserByContribution(contribution);

            if (userLike != null)
            {
                if (userLike.Enabled)
                {
                    return null; // user already liked contribution
                }
                allowNotification = false; // Already been emailed before, don't do it again
                userLike = _userRepo.EnableContributionLike(userLike);
            }
            else
            {
                userLike = new ContributionLike(User, contribution);
                _userRepo.AddContributionLike(userLike);
            }

            contribution = _contributionRepo.IncrementContributionLikes(contribution);

            if (allowNotification && User.Id != contributionAuthor.Id && RobeazyNotifications.UserRequiresNotification(contributionAuthor, RobeazyNotifications.NotificationType.EmailOnLikedContribution))
            {
                RobeazyNotifications.SendEmailNotification(contributionAuthor, RobeazyNotifications.NotificationType.EmailOnLikedContribution, contribution.Story, contribution);
            }

            return new ContributionLikeResult(userLike, contribution);
        }

        public ContributionLikeResult UnlikeContributionLike(int contributionId)
        {
            var contribution = _contributionRepo.GetContributionById(contributionId);
            if (contribution == null)
            {
                return null;
            }

            var userLike = GetUserContributionLikeByUserByContribution(contribution);

            if (userLike == null)
            {
                return null;
            }

            userLike = _userRepo.DisableContributionLike(userLike);
            contribution = _contributionRepo.DecrementContributionLikes(contribution);

            return new ContributionLikeResult(userLike, contribution);
        }

        public StoryLikeResult CreateStoryLike(int storyId)
        {
            var story = _storyRepo.GetStoryById(storyId);
            if (story == null)
            {
                return null;
            }
            var storyAuthor = story.Author;
            if (User.Id == storyAuthor.Id)
            {
                return null;
            }
            bool allowNotification = true; // Prevent like -> unlike -> like email spam
            var userLike = GetUserStoryLikeByUserByStory(story);
            if (userLike != null)
            {
                if (userLike.Enabled)
                {
                    return null;
                }
                allowNotification = false; // Already been emailed before, don't do it again
                userLike = _userRepo.EnableStoryLike(userLike);
            }
            else
            {
                userLike = new StoryLike(User, story);
                _userRepo.AddStoryLike(userLike);
            }

            story = _storyRepo.IncrementStoryLikes(story);

            if (allowNotification && User.Id != storyAuthor.Id && RobeazyNotifications.UserRequiresNotification(storyAuthor, RobeazyNotifications.NotificationType.EmailOnLikedStory))
            {
                RobeazyNotifications.SendEmailNotification(storyAuthor, RobeazyNotifications.NotificationType.EmailOnLikedStory, story);
            }

            return new StoryLikeResult(userLike, story);
        }


        public StoryLikeResult UnlikeStoryLike(int contributionId)
        {
            var story = _storyRepo.GetStoryById(contributionId);
            if (story == null)
            {
                return null;
            }

            var userLike = GetUserStoryLikeByUserByStory(story);

            if (userLike == null)
            {
                return null;
            }

            userLike = _userRepo.DisableStoryLike(userLike);
            story = _storyRepo.DecrementStoryLikes(story);

            return new StoryLikeResult(userLike, story);
        }

        public ContributionLike GetUserContributionLikeByUserByContribution(Contribution contribution)
        {
            return _userRepo.GetUserContributionLikeByUserByContribution(User, contribution);
        }

        public StoryLike GetUserStoryLikeByUserByStory(Story story)
        {
            return _userRepo.GetUserStoryLikeByUserByStory(User, story);
        }

        public List<ContributionLikeViewModel> GetUserLikedContributions()
        {
            return _userRepo.GetUserLikedContributions(User);
        }

        public List<ContributionLikeViewModel> GetUserLikedContributions(ApplicationUser user)
        {
            return _userRepo.GetUserLikedContributions(user);
        }

        public List<StoryViewModel> GetUserLikedStories()
        {
            return _userRepo.GetUserLikedStories(User);
        }

        public List<StoryViewModel> GetUserLikedStories(ApplicationUser user)
        {
            return _userRepo.GetUserLikedStories(user);
        }

        public Report GetContributionReportByUserByContribution(Contribution contribution)
        {
            return _userRepo.GetContributionReportByUserByContribution(User, contribution);
        }

        public Report GetStoryReportByUserByStory(Story story)
        {
            return _userRepo.GetStoryReportByUserByStory(User, story);
        }
    }
}