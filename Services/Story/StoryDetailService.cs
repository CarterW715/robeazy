using RobeazyCore.Models;
using RobeazyCore.Respository;
using System.Collections.Generic;

namespace RobeazyCore.Services
{
    public class StoryDetailService : IStoryDetailService
    {
        private readonly IStoryRepository _storyRepo;

        public StoryDetailService(IStoryRepository storyRepo)
        {
            _storyRepo = storyRepo;
        }

        public List<StoryCompositeViewModel> GetAllStories(StoryType? storyType)
            => _storyRepo.GetAllStoryViewModels(storyType);
    }
}