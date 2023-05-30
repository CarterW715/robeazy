using RobeazyCore.Data;
using RobeazyCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace RobeazyCore.Respository
{
    public class StoryRepository : IStoryRepository
    {
        private readonly ApplicationDbContext _context;

        public StoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser GetUser(string id)
        {
            return _context.Users.Find(id);
        }

        public List<StoryCompositeViewModel> GetAllStoryViewModels(StoryType? storyType)
        {
            var storyQuery = (from story in _context.Stories
                              join textStory in _context.TextStories on story.Id equals textStory.StoryId
                              join guitarTab in _context.GuitarTabs on story.Id equals guitarTab.StoryId
                              join movieScript in _context.MovieScripts on story.Id equals movieScript.StoryId
                              orderby story.Upvotes descending, story.CreateDate descending
                              select new StoryCompositeViewModel
                              {
                                  Story = story,
                                  TextStory = textStory,
                                  GuitarTab = guitarTab,
                                  MovieScript = movieScript
                              });

            if (storyType != null)
            {
                storyQuery = storyQuery.Where(s => s.Story.StoryType == storyType);
            }

            return storyQuery.ToList();
        }

        public List<StoryViewModel> GetStoryViewModelsByUser(ApplicationUser user)
        {
            var stories = (from story in _context.Stories
                           where story.Author.Id == user.Id
                           orderby story.CreateDate descending
                           select new StoryViewModel
                           {
                               StoryId = story.Id,
                               Title = story.Title,
                               Author = story.Author.UserName,
                               Upvotes = story.Upvotes,
                               Downvotes = story.Downvotes,
                               CreateDate = story.CreateDate,
                               Liked = false,
                               Reported = false
                           }).ToList();
            return stories;
        }

        public Story GetStoryById(int storyId)
         => _context.Stories.Find(storyId);

        public Story IncrementStoryLikes(Story story)
        {
            _context.Stories.Attach(story);
            story.Upvotes++;
            _context.SaveChangesAsync();

            return story;
        }

        public Story DecrementStoryLikes(Story story)
        {
            _context.Stories.Attach(story);
            if (story.Upvotes > 0)
            {
                story.Upvotes--;
            }
            _context.SaveChangesAsync();

            return story;
        }
    }
}