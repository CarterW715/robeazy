using Microsoft.AspNetCore.Mvc;
using RobeazyCore.Models;
using RobeazyCore.Services;

namespace RobeazyCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStoryDetailService _storyDetailService;

        public HomeController(IStoryDetailService storyDetailService)
        {
            _storyDetailService = storyDetailService;
        }

        public ActionResult Index(StoryType? storyType)
        {
            var viewModels = _storyDetailService.GetAllStories(storyType);
            return Json(new { success = true, stories = viewModels });
        }
    }
}