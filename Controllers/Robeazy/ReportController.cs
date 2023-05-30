using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using RobeazyCore.Data;
using RobeazyCore.Models;
using RobeazyCore.Services;

namespace RobeazyCore.Controllers
{
    public class ReportController : Controller
    {
        private IUserService _userService { get; set; }

        public ReportController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("ReportContribution")]
        [ValidateAntiForgeryToken]
        public ActionResult ReportContribution(ReportRequestModel request)
        {
            if (ModelState.IsValid)
            {
                var report = _userService.ReportContribution(request);
                if (report != null)
                {
                    return Json(new { success = true, responseText = "Your report was successfuly sent!" });
                }
            }

            return Json(new { success = false, responseText = "An error has occured. Try again later." });
        }

        [HttpPost, ActionName("ReportStory")]
        [ValidateAntiForgeryToken]
        public ActionResult ReportStory(ReportRequestModel request)
        {
            if (ModelState.IsValid)
            {
                var report = _userService.ReportStory(request);
                if (report != null)
                {
                    return Json(new { success = true, responseText = "Your report was successfuly sent!" });
                }
            }

            return Json(new { success = false, responseText = "An error has occured. Try again later." });
        }
    }
}