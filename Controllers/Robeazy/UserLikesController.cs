using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using RobeazyCore.Data;
using RobeazyCore.Models;
using RobeazyCore.Services;

namespace RobeazyCore.Controllers
{
    public class UserLikesController : Controller
    {
        private readonly IUserService _userService;

        public UserLikesController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateContributionLike(int contributionId)
        {
            if (ModelState.IsValid)
            {
                var userLike = _userService.CreateContributionLike(contributionId);
                if (userLike != null && userLike.Success)
                {
                    return Json(new { success = true, responseText = "Contribution was successfuly liked!", newScore = (userLike.Contribution.Upvotes - userLike.Contribution.Downvotes) });
                }
            }

            return Json(new { success = false, responseText = "An error has occured. Try again later." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteContributionLike(int contributionId)
        {
            if (ModelState.IsValid)
            {
                var userLike = _userService.CreateContributionLike(contributionId);
                if (userLike != null && userLike.Success)
                {
                    return Json(new { success = true, responseText = "Contribution was successfuly unliked!", newScore = (userLike.Contribution.Upvotes - userLike.Contribution.Downvotes) });
                }
            }

            return Json(new { success = false, responseText = "An error has occured. Try again later." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStoryLike(int storyId)
        {
            if (ModelState.IsValid)
            {
                var userLike = _userService.CreateStoryLike(storyId);
                if (userLike != null && userLike.Success)
                {
                    return Json(new { success = true, responseText = "Your like was successfuly sent!", newScore = (userLike.Story.Upvotes - userLike.Story.Downvotes) });
                }
            }

            return Json(new { success = false, responseText = "An error has occured. Try again later." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStoryLike(int storyId)
        {
            if (ModelState.IsValid)
            {
                var userLike = _userService.UnlikeStoryLike(storyId);
                if (userLike != null && userLike.Success)
                {
                    return Json(new { success = true, responseText = "Your like was successfuly removed!", newScore = (userLike.Story.Upvotes - userLike.Story.Downvotes) });
                }
            }

            return Json(new { success = false, responseText = "An error has occured. Try again later." });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
