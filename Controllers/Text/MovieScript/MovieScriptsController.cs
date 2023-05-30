using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RobeazyCore.Models;
using RobeazyCore.Services;

namespace RobeazyCore.Controllers
{
    public class MovieScriptsController : Controller
    {
        private readonly IStoryService<MovieScript, MovieScriptModel, MovieScriptContributionAggregate, MovieScriptContributionView> _storyService;
        private readonly IUserService _userService;

        public MovieScriptsController(IStoryService<MovieScript, MovieScriptModel, MovieScriptContributionAggregate, MovieScriptContributionView> storyService, IUserService userService)
        {
            _storyService = storyService;
            _userService = userService;
        }

        // GET
        public ActionResult Details(string title, string[] contributionIds)
        {
            var storyResults = _storyService.GetStoryDetails(title, _userService.User, contributionIds);
            var view = storyResults.Story;
            view.ContributionViewModels = storyResults.ContributionViews;
            return View(view);
            #region old code
            //ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            //Story story = db.Stories.FirstOrDefault(s => s.Title == title);
            //if (story == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //MovieScript script = db.MovieScripts.FirstOrDefault(x => x.Story.StoryId == story.StoryId);
            //if (script == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            //int HighlightId = 0;

            //List<MovieScriptContributionView> ContributionViewModels = new List<MovieScriptContributionView>();

            //MovieScriptContributionQueries query = new MovieScriptContributionQueries(db);

            //if (contributionIds == null || contributionIds.Length == 0)
            //{
            //    MovieScriptContributionView ContributionViewModel = query.GetFirstContribution(script.Story, user);

            //    if (ContributionViewModel == null)
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }

            //    while (ContributionViewModel != null)
            //    {
            //        // Add the prior contribution contents to a view model and add it to the view
            //        ContributionViewModels.Add(ContributionViewModel);

            //        // Populate with the next, highest scoring contribution replied to the previous contribution
            //        ContributionViewModel = query.GetNextHighestScoringContribution(ContributionViewModel.ContributionId, user);
            //    }
            //}
            ////else
            ////{
            ////    Int32 id = Int32.Parse(contributionIds[0]);
            ////    TextStoryContribution Contribution = db.TextStoryContributions.Find(id);
            ////    if (Contribution == null)
            ////    {
            ////        return Json(new { success = false, responseText = "Contribution was not found" }, JsonRequestBehavior.DenyGet);
            ////    }

            ////    TextStoryContributionView ContributionViewModel = TextStoryContributionView.ContributionToViewModel(Contribution, db, user);
            ////    HighlightId = id;
            ////    ContributionViewModels.Add(ContributionViewModel);

            ////    // Get up the chain all of the way to the root of the story
            ////    TextStoryContribution PreviousContribution = Contribution.PreviousContribution;
            ////    while (PreviousContribution != null)
            ////    {
            ////        TextStoryContributionView PreviuousContributionViewModel = TextStoryContributionView.ContributionToViewModel(PreviousContribution, db, user);
            ////        ContributionViewModels.Insert(0, PreviuousContributionViewModel);
            ////        PreviousContribution = PreviousContribution.PreviousContribution; // Go up the chain
            ////    }

            ////    // Now add the other inputed contributions
            ////    for (int count = 1; count < contributionIds.Length; count++)
            ////    {
            ////        id = Int32.Parse(contributionIds[count]);
            ////        Contribution = db.TextStoryContributions.Find(id);
            ////        if (Contribution == null)
            ////        {
            ////            return Json(new { success = false, responseText = "Contribution was not found" }, JsonRequestBehavior.DenyGet);
            ////        }

            ////        ContributionViewModel = TextStoryContributionView.ContributionToViewModel(Contribution, db, user);
            ////        ContributionViewModels.Add(ContributionViewModel);
            ////    }

            ////    // Finish out the rest of the story's contributions on this branch
            ////    ContributionViewModel = query.GetNextHighestScoringContribution(ContributionViewModel.ContributionId, user);

            ////    while (ContributionViewModel != null)
            ////    {
            ////        ContributionViewModels.Add(ContributionViewModel);
            ////        ContributionViewModel = query.GetNextHighestScoringContribution(ContributionViewModel.ContributionId, user);
            ////    }

            ////}

            //StoryLike userLike = null;
            //Report userReport = null;
            //if (user != null)
            //{
            //    userLike = UserLikeQueries.GetUserStoryLikeByUserByStory(db, user, story);
            //    userReport = UserReportQueries.GetUserTextReportByUserByStory(db, user, story);
            //}

            //MovieScriptModel viewStory = new MovieScriptModel
            //{
            //    Story = story,
            //    Script = script,
            //    HighlightId = HighlightId,
            //    ContributionViewModels = ContributionViewModels,
            //    Liked = userLike != null,
            //    Reported = userReport != null
            //};

            //return View(viewStory);
            #endregion
        }

        // GET: Text/Create
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CreateMovieScriptModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateMovieScriptModel scriptRequest)
        {
            if (_userService.User == null)
            {
                return Json(new { success = false, responseText = "User is not logged in" });
            }

            var storyResults = _storyService.CreateStory(scriptRequest, _userService.User);

            if (!storyResults.Success)
            {
                return Json(new { success = false, errors = storyResults.GetErrors() });
            }

            return RedirectToAction("Details", new { title = scriptRequest.Title });
            #region old code
            //if (ModelState.IsValid)
            //{
            //    string userId = User.Identity.GetUserId();
            //    if (userId == null)
            //    {
            //        return Json(new { success = false, responseText = "User is not logged in" }, JsonRequestBehavior.DenyGet);
            //    }
            //    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            //    if (user == null)
            //    {
            //        string errorMsg = String.Format("User not found in the database");
            //        ModelState.AddModelError(string.Empty, errorMsg);
            //    }
            //    else
            //    {
            //        List<MovieScriptContributionCreate> Elements = ElementJsonConverter.ElementJsonConvert(script.ElementJson); // TODO: find better way to do this...
            //        if (Elements.Count > script.ElementMax || Elements.Count < script.ElementMin)
            //        {
            //            string errorMsg = $"The number of script elements must be between {script.ElementMin} and {script.ElementMax}";
            //            ModelState.AddModelError(string.Empty, errorMsg);
            //        }
            //        try
            //        {

            //            MovieScript newScript = new MovieScript(script.Title, user, script.ElementMax, script.DynamicContributions, script.Genre);

            //            List<MovieScriptContribution> firstContribution = MovieScriptContribution.CreateAggregateContribution(user, newScript.Story, Elements);

            //            db.MovieScripts.Add(newScript);
            //            db.MovieScriptContributions.AddRange(firstContribution);
            //            db.SaveChanges();
            //            //return RedirectToAction("Details", new { title = script.Title });
            //            return RedirectToAction("Index", "Home");
            //        }
            //        catch (Exception e)
            //        {
            //            var test = e.Message;
            //            // do something
            //        }
            //    }
            //}

            //return Json(new { success = false, responseText = "Unable to process request, please try again." }, JsonRequestBehavior.DenyGet);
            #endregion
        }     

        protected override void Dispose(bool disposing)
        {
            if (disposing) { }
            base.Dispose(disposing);
        }
    }
}
