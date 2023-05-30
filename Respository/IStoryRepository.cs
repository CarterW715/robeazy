using RobeazyCore.Models;
using System.Collections.Generic;

namespace RobeazyCore.Respository
{
    public interface IStoryRepository
    {
        List<StoryCompositeViewModel> GetAllStoryViewModels(StoryType? storyType);
        List<StoryViewModel> GetStoryViewModelsByUser(ApplicationUser user);
        Story GetStoryById(int storyId);
        Story IncrementStoryLikes(Story story);
        Story DecrementStoryLikes(Story story);
    }
}