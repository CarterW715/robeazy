using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using RobeazyCore.Data;
using RobeazyCore.Models;
using RobeazyCore.Services;

namespace RobeazyCore.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IUserService userService, IFeedbackService feedbackService)
        {
            _userService = userService;
            _feedbackService = feedbackService;
        }

        [HttpGet]
        public ActionResult ReportBug()
        {
            return View("~/Views/Feedback/ReportBug.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportBug(FeedbackRequestModel request)
        {
            if (ModelState.IsValid)
            {
                var feedback = _feedbackService.CreateBug(request, _userService.User);
                if (feedback != null)
                {
                    return Json(new { success = true, responseText = "Your bug was successfuly reported" });
                }
            }
            return Json(new { success = false, responseText = "An error has occured. Try again later." });
        }

        [HttpGet]
        public ActionResult SuggestionBox()
        {
            return View("~/Views/Feedback/Suggestion.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitSuggestion(FeedbackRequestModel request)
        {
            if (ModelState.IsValid)
            {
                var feedback = _feedbackService.CreateSuggestion(request, _userService.User);
                if (feedback != null)
                {
                    return Json(new { success = true, responseText = "Your suggestion was successfuly reported" });
                }
            }
            return Json(new { success = false, responseText = "An error has occured. Try again later." });
        }
    }
}