using Microsoft.AspNetCore.Mvc;
using RobeazyCore.Models;
using RobeazyCore.Services;
using RobeazyCore.Variables;
using System.Linq;
using System.Net;

namespace RobeazyCore.Controllers
{
    public class GuitarTabContributionsController : Controller
    {
        private readonly IContributionService<GuitarTab, GuitarTabViewModel, GuitarTabContribution, GuitarTabContributionView> _contributionService;
        private readonly IUserService _userService;

        public GuitarTabContributionsController(IContributionService<GuitarTab, GuitarTabViewModel, GuitarTabContribution, GuitarTabContributionView> contributionService, IUserService userService)
        {
            _contributionService = contributionService;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult GetRelatedContributions(RepliesRequest request)
        {
            var contributionResults = _contributionService.GetRelatedContributions<GuitarTabContributionReplyViewModel>(request, GlobalVar.GUITARTAB_CONTRIBUTION_RECORD_COUNT, _userService.User);
            var viewModel = new GuitarTabContributionReplyViewModel(contributionResults.ContributionViewModels, request.Depth);
            return Json(new { success = true, responseText = "Success", contributionIds = contributionResults.ContributionIds, contributionHtml = RenderRazorViewToString("_ContributionRepliesPartial", viewModel) });

            #region old code
            //int recordCount = GlobalVar.GUITARTAB_CONTRIBUTION_RECORD_COUNT;
            //ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            //GuitarTabContributionQueries query = new GuitarTabContributionQueries(db);
            //List<GuitarTabContributionView> contributions = query.GetContributionReplies(request.CurrentContributionId, request.PreviousContributionId, request.IdBlackList, recordCount, true, user);
            //GuitarTabContributionReplyViewModel viewModel = new GuitarTabContributionReplyViewModel(contributions, request.Depth);
            //return Json(new { success = true, responseText = "Success", contributionIds = contributions.ConvertAll(c => c.ContributionId), contributionHtml = RenderRazorViewToString("_GuitarTabContributionRepliesPartial", viewModel) }, JsonRequestBehavior.AllowGet);
            #endregion
        }

        [HttpGet]
        public ActionResult ViewMoreContributions(ViewMoreRequestViewModel request)
        {
            var contributionResults = _contributionService.ViewMoreContributions(request, _userService.User);
            var contributionHtml = "";
            while (contributionResults.ContributionViews.Count > 0)
            {
                contributionHtml += RenderRazorViewToString("_ContributionPartial", new GuitarTabContributionFullViewModel(contributionResults.ContributionViews.Dequeue(), request.Depth));
            }
            return Json(new
            {
                success = true,
                responseText = "Success",
                contributionHtml,
                contributionResults.IsLastContribution,
                contributionResults.LastContributionId,
                contributionResults.ContributionIds
            });

            #region old code
            //int recordCount = GlobalVar.TEXT_CONTRIBUTION_RECORD_COUNT;
            //ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            //GuitarTabContributionQueries query = new GuitarTabContributionQueries(db);
            //List<GuitarTabContributionView> ContributionViewModels = new List<GuitarTabContributionView>();
            //GuitarTabContributionView model = null;
            //model = query.GetNextHighestScoringContribution(request.CurrentContributionId, user);
            //int count = 1;
            //string contributionHtml = "";
            //bool isLastContribution = true;
            //int lastContributionId = 0;
            //Stack<int> contributionIds = new Stack<int>();
            //while (model != null)
            //{
            //    contributionIds.Push(model.ContributionId);
            //    lastContributionId = model.ContributionId;
            //    if (count > recordCount)
            //    {
            //        model = query.GetNextHighestScoringContribution(model.ContributionId, user);
            //        isLastContribution = model == null;
            //        break;
            //    }
            //    else
            //    {
            //        // Add the prior contribution contents to a view model and add it to the view
            //        contributionHtml += RenderRazorViewToString("_GuitarTabContributionPartial", new GuitarTabContributionFullViewModel(model, request.Depth));

            //        // Populate with the next, highest scoring contribution replied to the previous contribution
            //        model = query.GetNextHighestScoringContribution(model.ContributionId, user);

            //        count++;
            //    }
            //}

            //return Json(new
            //{
            //    success = true,
            //    responseText = "Success",
            //    contributionHtml,
            //    isLastContribution,
            //    lastContributionId,
            //    contributionIds
            //}, JsonRequestBehavior.AllowGet);
            #endregion
        }

        [HttpGet]
        public ActionResult GetCreateContributionView(CreateContributionRequest<GuitarTab> request)
        {
            if (_userService.User == null)
            {
                return Json(new { success = false, responseText = "User is not logged in" });
            }
            var viewModel = new CreateGuitarTabContributionViewModel(request);
            return Json(new { success = true, responseText = "Success", contributionHtml = RenderRazorViewToString("_CreateGuitarTabContributionPartial", viewModel) });
        }

        // POST: TextContributions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateGuitarTabContributionViewModel request)
        {
            if (_userService.User == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { success = false, responseText = "User not found in the database" });
            }

            var contributionResult = _contributionService.CreateContribution(request, _userService.User);
            if (!contributionResult.Success)
            {
                return Json(new { success = false, responseText = contributionResult.GetErrors()});
            }

            string contributionHtml;
            if (request.IsContinueStory)
            {
                contributionHtml = RenderRazorViewToString("_ContributionPartial", new GuitarTabContributionFullViewModel(contributionResult.Contribution, request.Depth));
            }
            else
            {
                var viewModel = new GuitarTabContributionReplyViewModel(contributionResult.Contribution, request.Depth);
                viewModel.Contributions.First().LastContributionId = request.TargetContributionId;
                contributionHtml = RenderRazorViewToString("_ContributionRepliesPartial", viewModel);
            }

            return Json(new
            {
                success = true,
                responseText = "Your contribution was successfuly created!",
                isContinueStory = request.IsContinueStory,
                contributionHtml,
                targetContributionId = request.TargetContributionId, // The element id
                contributionId = contributionResult.Contribution.ContributionId,
                depth = request.Depth
            });

            #region old code
            //if (ModelState.IsValid)
            //{
            //    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            //    if (user == null)
            //    {
            //        Response.StatusCode = (int)HttpStatusCode.NotFound;
            //        return Json(new { success = false, responseText = "User not found in the database" }, JsonRequestBehavior.DenyGet);
            //    }
            //    GuitarTab story = db.GuitarTabs.FirstOrDefault(s => s.Story.StoryId == request.StoryId);
            //    if (story == null)
            //    {
            //        Response.StatusCode = (int)HttpStatusCode.NotFound;
            //        return Json(new { success = false, responseText = "The story that is being contributed to cannot be found in the database" }, JsonRequestBehavior.DenyGet);
            //    }
            //    request.GuitarOne = request.GuitarOne.Trim();
            //    GuitarTabContribution PreviousContribution = db.GuitarTabContributions.Where(c => c.Contribution.ContributionId == request.TargetContributionId).FirstOrDefault();
            //    if (PreviousContribution == null)
            //    {
            //        Response.StatusCode = (int) HttpStatusCode.NotFound;
            //        return Json(new { success = false, responseText = "The contribution that is being replied to cannot be found in the database" }, JsonRequestBehavior.DenyGet);
            //    }
            //    //request.Content = request.Content.Replace("\r\n", "<br>");
            //    GuitarTabContribution newContribution = new GuitarTabContribution(user, story.Story, PreviousContribution, request.GuitarOne);
            //    db.GuitarTabContributions.Add(newContribution);
            //    db.SaveChanges();
            //    // Check if the user being replied to needs an email notification
            //    ApplicationUser ContributionAuthor = PreviousContribution.Contribution.Contributor;
            //    if (user.Id != ContributionAuthor.Id)
            //    {
            //        if (RobeazyNotifications.UserRequiresNotification(ContributionAuthor, RobeazyNotifications.NotificationType.EmailOnCreatedContribution))
            //        {
            //            RobeazyNotifications.SendEmailNotification(ContributionAuthor, RobeazyNotifications.NotificationType.EmailOnCreatedContribution, story.Story, newContribution.Contribution);
            //        }
            //        // Check if the story's author needs an email notification
            //        ApplicationUser StoryAuthor = story.Story.Author;
            //        if (user.Id != StoryAuthor.Id && ContributionAuthor.Id != StoryAuthor.Id && RobeazyNotifications.UserRequiresNotification(StoryAuthor, RobeazyNotifications.NotificationType.EmailOnCreatedStory))
            //        {
            //            RobeazyNotifications.SendEmailNotification(StoryAuthor, RobeazyNotifications.NotificationType.EmailOnCreatedStory, story.Story, newContribution.Contribution);
            //        }
            //    }         
            //    string contributionHtml = "";
            //    if (request.IsContinueStory)
            //    {
            //        GuitarTabContributionFullViewModel viewModel = new GuitarTabContributionFullViewModel(GuitarTabContributionView.ContributionToViewModel(newContribution), request.Depth);
            //        contributionHtml = RenderRazorViewToString("_GuitarTabContributionPartial", viewModel);
            //    }
            //    else
            //    {
            //        GuitarTabContributionReplyViewModel viewModel = new GuitarTabContributionReplyViewModel(new List<GuitarTabContributionView>() { GuitarTabContributionView.ContributionToViewModel(newContribution) }, request.Depth);
            //        viewModel.Contributions.First().LastContributionId = request.TargetContributionId;
            //        contributionHtml = RenderRazorViewToString("_GuitarTabContributionRepliesPartial", viewModel);
            //    }
            //    return Json(new
            //    {
            //        success = true,
            //        responseText = "Your contribution was successfuly created!",
            //        isContinueStory = request.IsContinueStory,
            //        contributionHtml = contributionHtml,
            //        targetContributionId = request.TargetContributionId, // The element id
            //        contributionId = newContribution.Contribution.ContributionId,
            //        depth = request.Depth
            //    }, JsonRequestBehavior.AllowGet);
            //}
            //return Json(new { success = false, responseText = "An error has occured. Try again later." }, JsonRequestBehavior.DenyGet);
            #endregion
        }   

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }

        // Move to utility class
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                //var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                //var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                //viewResult.View.Render(viewContext, sw);
                //viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                //return sw.GetStringBuilder().ToString();
                return "";
            }
        }
    }
}
