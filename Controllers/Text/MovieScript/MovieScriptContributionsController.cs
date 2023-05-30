using Microsoft.AspNetCore.Mvc;
using RobeazyCore.Models;
using RobeazyCore.Variables;
using RobeazyCore.Services;
using System.Linq;
using System.Net;

namespace RobeazyCore.Controllers
{
    public class MovieScriptContributionsController : Controller
    {
        private readonly IContributionService<MovieScript, MovieScriptModel, MovieScriptContributionAggregate, MovieScriptContributionView> _contributionService;
        private readonly IUserService _userService;

        public MovieScriptContributionsController(IContributionService<MovieScript, MovieScriptModel, MovieScriptContributionAggregate, MovieScriptContributionView> contributionService, IUserService userService)
        {
            _contributionService = contributionService;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult GetRelatedContributions(RepliesRequest request)
        {
            var contributionResults = _contributionService.GetRelatedContributions<MovieScriptContributionReplyViewModel>(request, GlobalVar.MOVIESCRIPT_CONTRIBUTION_RECORD_COUNT, _userService.User);
            var viewModel = new MovieScriptContributionReplyViewModel(contributionResults.ContributionViewModels, request.Depth);
            return Json(new { success = true, responseText = "Success", contributionIds = contributionResults.ContributionIds, contributionHtml = RenderRazorViewToString("_ContributionRepliesPartial", viewModel) });
            #region old code
            //int recordCount = GlobalVar.MOVIESCRIPT_CONTRIBUTION_RECORD_COUNT;
            //ApplicationUser user = _db.Users.Find(User.Identity.GetUserId());
            //List<MovieScriptContributionView> contributions = _movieScriptQuery.GetContributionReplies(request.CurrentContributionId, request.PreviousContributionId, request.IdBlackList, recordCount, true, user);
            //MovieScriptContributionReplyViewModel viewModel = new MovieScriptContributionReplyViewModel(contributions, request.Depth);
            //return Json(new { success = true, responseText = "Success", contributionIds = contributions.ConvertAll(c => c.ContributionId), contributionHtml = RenderRazorViewToString("_ContributionRepliesPartial", viewModel) }, JsonRequestBehavior.AllowGet);
            #endregion
        }

        [HttpGet]
        public ActionResult ViewMoreContributions(ViewMoreRequestViewModel request)
        {
            var contributionResults = _contributionService.ViewMoreContributions(request, _userService.User);
            var contributionHtml = "";
            while (contributionResults.ContributionViews.Count > 0)
            {
                contributionHtml += RenderRazorViewToString("_ContributionPartial", new MovieScriptContributionFullViewModel(contributionResults.ContributionViews.Dequeue(), request.Depth));
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
            //int recordCount = GlobalVar.MOVIESCRIPT_CONTRIBUTION_RECORD_COUNT;
            //ApplicationUser user = _db.Users.Find(User.Identity.GetUserId());
            //List<MovieScriptContributionView> ContributionViewModels = new List<MovieScriptContributionView>();
            //MovieScriptContributionView model = null;
            //model = _movieScriptQuery.GetNextHighestScoringContribution(request.CurrentContributionId, user);
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
            //        model = _movieScriptQuery.GetNextHighestScoringContribution(model.ContributionId, user);
            //        isLastContribution = model == null;
            //        break;
            //    }
            //    else
            //    {
            //        // Add the prior contribution contents to a view model and add it to the view
            //        contributionHtml += RenderRazorViewToString("_ContributionPartial", new MovieScriptContributionFullViewModel(model, request.Depth));

            //        // Populate with the next, highest scoring contribution replied to the previous contribution
            //        model = _movieScriptQuery.GetNextHighestScoringContribution(model.ContributionId, user);

            //        count++;
            //    }
            //}

            //return Json(new
            //{
            //    success = true,
            //    responseText = "Success",
            //    contributionHtml = contributionHtml,
            //    isLastContribution = isLastContribution,
            //    lastContributionId = lastContributionId,
            //    contributionIds = contributionIds
            //}, JsonRequestBehavior.AllowGet);
            #endregion
        }

        [HttpGet]
        public ActionResult GetCreateContributionView(CreateContributionRequest<MovieScript> request)
        {
            if (_userService.User == null)
            {
                return Json(new { success = false, responseText = "User is not logged in" });
            }
            var viewModel = new CreateMovieScriptContributionViewModel(request);
            return Json(new { success = true, responseText = "Success", contributionHtml = RenderRazorViewToString("_CreateContributionPartial", viewModel) });
            #region old code
            //ApplicationUser user = _db.Users.Find(User.Identity.GetUserId());
            //if (user == null)
            //{
            //    return Json(new { success = false, responseText = "User is not logged in" }, JsonRequestBehavior.AllowGet);
            //}
            //CreateMovieScriptContributionViewModel viewModel = new CreateMovieScriptContributionViewModel(request);
            //return Json(new { success = true, responseText = "Success", contributionHtml = RenderRazorViewToString("_CreateContributionPartial", viewModel) }, JsonRequestBehavior.AllowGet);
            #endregion
        }

        // POST: TextContributions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateMovieScriptContributionViewModel request)
        {
            if (_userService.User == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { success = false, responseText = "User not found in the database" });
            }

            var contributionResult = _contributionService.CreateContribution(request, _userService.User);
            if (!contributionResult.Success)
            {
                return Json(new { success = false, responseText = contributionResult.GetErrors() });
            }

            string contributionHtml;
            if (request.IsContinueStory)
            {
                contributionHtml = RenderRazorViewToString("_ContributionPartial", new MovieScriptContributionFullViewModel(contributionResult.Contribution, request.Depth));
            }
            else
            {
                var viewModel = new MovieScriptContributionReplyViewModel(contributionResult.Contribution, request.Depth);
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
                contributionId = contributionResult.Contribution.Id,
                depth = request.Depth
            });
            #region old code
            //if (ModelState.IsValid)
            //{
            //    ApplicationUser user = _db.Users.Find(User.Identity.GetUserId());
            //    if (user == null)
            //    {
            //        Response.StatusCode = (int)HttpStatusCode.NotFound;
            //        return Json(new { success = false, responseText = "User not found in the database" }, JsonRequestBehavior.DenyGet);
            //    }
            //    MovieScript script = _db.MovieScripts.FirstOrDefault(s => s.Story.StoryId == request.StoryId);
            //    if (script == null)
            //    {
            //        Response.StatusCode = (int)HttpStatusCode.NotFound;
            //        return Json(new { success = false, responseText = "The story that is being contributed to cannot be found in the database" }, JsonRequestBehavior.DenyGet);
            //    }
            //    request.Content = request.Content.Trim();
            //    List<MovieScriptContributionCreate> Elements = ElementJsonConverter.ElementJsonConvert(request.Content);
            //    if (Elements.Count > script.ElementMax || Elements.Count < script.ElementMin)
            //    {
            //        string errorMsg = $"The number of script elements must be between {script.ElementMin} and {script.ElementMax}";
            //        return Json(new { success = false, errorCode = 1000, responseText = errorMsg }, JsonRequestBehavior.DenyGet);
            //    }
            //    MovieScriptContribution previousContribution = _db.MovieScriptContributions.Where(c => c.Contribution.ContributionId == request.TargetContributionId).FirstOrDefault();
            //    if (previousContribution == null)
            //    {
            //        Response.StatusCode = (int)HttpStatusCode.NotFound;
            //        return Json(new { success = false, responseText = "The contribution that is being replied to cannot be found in the database" }, JsonRequestBehavior.DenyGet);
            //    }
            //    MovieScriptContributionAggregate newContribution = new MovieScriptContributionAggregate(user, script.Story, previousContribution.Contribution, Elements);
            //    _db.MovieScriptContributions.AddRange(newContribution.Elements);
            //    _db.SaveChanges();
            //    // Check if the user being replied to needs an email notification
            //    ApplicationUser contributionAuthor = previousContribution.Contribution.Contributor;
            //    if (user.Id != contributionAuthor.Id)
            //    {
            //        if (RobeazyNotifications.UserRequiresNotification(contributionAuthor, RobeazyNotifications.NotificationType.EmailOnCreatedContribution))
            //        {
            //            RobeazyNotifications.SendEmailNotification(contributionAuthor, RobeazyNotifications.NotificationType.EmailOnCreatedContribution, script.Story, newContribution.Contribution);
            //        }
            //        // Check if the story's author needs an email notification
            //        ApplicationUser StoryAuthor = script.Story.Author;
            //        if (user.Id != StoryAuthor.Id && contributionAuthor.Id != StoryAuthor.Id && RobeazyNotifications.UserRequiresNotification(StoryAuthor, RobeazyNotifications.NotificationType.EmailOnCreatedStory))
            //        {
            //            RobeazyNotifications.SendEmailNotification(StoryAuthor, RobeazyNotifications.NotificationType.EmailOnCreatedStory, script.Story, newContribution.Contribution);
            //        }
            //    }         
            //    string contributionHtml = "";
            //    if (request.IsContinueStory)
            //    {
            //        MovieScriptContributionFullViewModel viewModel = new MovieScriptContributionFullViewModel(MovieScriptContributionView.ElementsToContributionView(newContribution.Elements), request.Depth);
            //        contributionHtml = RenderRazorViewToString("_ContributionPartial", viewModel);
            //    }
            //    else
            //    {
            //        MovieScriptContributionReplyViewModel viewModel = new MovieScriptContributionReplyViewModel(new List<MovieScriptContributionView>() { MovieScriptContributionView.ElementsToContributionView(newContribution.Elements) }, request.Depth);
            //        viewModel.Contributions.First().LastContributionId = request.TargetContributionId;
            //        contributionHtml = RenderRazorViewToString("_ContributionRepliesPartial", viewModel);
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
            if (disposing) { }
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
