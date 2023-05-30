using RobeazyCore.Models;
using System.Collections.Generic;

namespace RobeazyCore.Services
{
    public interface IStoryDetailService
    {
        List<StoryCompositeViewModel> GetAllStories(StoryType? storyType);
    }
}
