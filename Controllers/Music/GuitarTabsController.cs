using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RobeazyCore.Models;
using RobeazyCore.Services;
using System.Threading.Tasks;

namespace RobeazyCore.Controllers.Music
{
    public class GuitarTabsController : Controller
    {
        private readonly IStoryService<GuitarTab, GuitarTabViewModel, GuitarTabContribution, GuitarTabContributionView> _storyService;
        private readonly IUserService _userService;

        public GuitarTabsController(IStoryService<GuitarTab, GuitarTabViewModel, GuitarTabContribution, GuitarTabContributionView> storyService, IUserService userService)
        {
            _storyService = storyService;
            _userService = userService;
        }

        public async Task<ActionResult> Details(string title, string[] contributionIds)
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
            //GuitarTab GuitarTab = db.GuitarTabs.FirstOrDefault(x => x.Story.StoryId == story.StoryId);
            //if (GuitarTab == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            //int HighlightId = 0;

            //List<GuitarTabContributionView> ContributionViewModels = new List<GuitarTabContributionView>();

            //GuitarTabContributionQueries query = new GuitarTabContributionQueries(db);

            //if (contributionIds == null || contributionIds.Length == 0)
            //{
            //    GuitarTabContributionView ContributionViewModel = query.GetFirstContribution(GuitarTab.Story, user);

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
            //else
            //{
            //    Int32 id = Int32.Parse(contributionIds[0]);
            //    GuitarTabContribution Contribution = db.GuitarTabContributions.Find(id);
            //    if (Contribution == null)
            //    {
            //        return Json(new { success = false, responseText = "Contribution was not found" }, JsonRequestBehavior.DenyGet);
            //    }

            //    GuitarTabContributionView ContributionViewModel = GuitarTabContributionView.ContributionToViewModel(Contribution, db, user);
            //    HighlightId = id;
            //    ContributionViewModels.Add(ContributionViewModel);

            //    // Get up the chain all of the way to the root of the story
            //    GuitarTabContribution PreviousContribution = Contribution.PreviousContribution;
            //    while (PreviousContribution != null)
            //    {
            //        GuitarTabContributionView PreviuousContributionViewModel = GuitarTabContributionView.ContributionToViewModel(PreviousContribution, db, user);
            //        ContributionViewModels.Insert(0, PreviuousContributionViewModel);
            //        PreviousContribution = PreviousContribution.PreviousContribution; // Go up the chain
            //    }

            //    // Now add the other inputed contributions
            //    for (int count = 1; count < contributionIds.Length; count++)
            //    {
            //        id = Int32.Parse(contributionIds[count]);
            //        Contribution = db.GuitarTabContributions.Find(id);
            //        if (Contribution == null)
            //        {
            //            return Json(new { success = false, responseText = "Contribution was not found" }, JsonRequestBehavior.DenyGet);
            //        }

            //        ContributionViewModel = GuitarTabContributionView.ContributionToViewModel(Contribution, db, user);
            //        ContributionViewModels.Add(ContributionViewModel);
            //    }

            //    // Finish out the rest of the story's contributions on this branch
            //    ContributionViewModel = query.GetNextHighestScoringContribution(ContributionViewModel.ContributionId, user);

            //    while (ContributionViewModel != null)
            //    {
            //        ContributionViewModels.Add(ContributionViewModel);
            //        ContributionViewModel = query.GetNextHighestScoringContribution(ContributionViewModel.ContributionId, user);
            //    }

            //}

            //StoryLike userLike = null;
            //Report userReport = null;
            //if (user != null)
            //{
            //    userLike = UserLikeQueries.GetUserStoryLikeByUserByStory(db, user, story);
            //    userReport = UserReportQueries.GetUserTextReportByUserByStory(db, user, story);
            //}

            //GuitarTabViewModel viewStory = new GuitarTabViewModel
            //{
            //    Story = story,
            //    Tab = GuitarTab,
            //    HighlightId = HighlightId,
            //    ContributionViewModels = ContributionViewModels,
            //    Liked = userLike != null,
            //    Reported = userReport != null
            //};

            //return View(viewStory);
            #endregion
        }

        // GET: GuitarTabs/Create
        [Authorize]
        public ActionResult Create()
        {
            return View(new CreateGuitarTabModel());
        }

        // POST: GuitarTabs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateGuitarTabModel guitarTabRequest)
        {
            if (_userService.User == null)
            {
                return Json(new { success = false, responseText = "User is not logged in" });
            }

            var storyResults = _storyService.CreateStory(guitarTabRequest, _userService.User);

            if (!storyResults.Success)
            {
                return Json(new { success = false, errors = storyResults.GetErrors() });
            }

            return RedirectToAction("Details", new { title = guitarTabRequest.Title });
            #region old code
            //if (ModelState.IsValid)
            //{
            //    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            //    if (user == null)
            //    {
            //        return Json(new { success = false, responseText = "User is not logged in" }, JsonRequestBehavior.DenyGet);
            //    }
            //    try
            //    {

            //        GuitarTab newGuitarTab = new GuitarTab(request, user);

            //        GuitarTabContribution newGuitarTabContribution = new GuitarTabContribution(user, newGuitarTab, request.GuitarOne);

            //        db.GuitarTabs.Add(newGuitarTab);
            //        db.GuitarTabContributions.Add(newGuitarTabContribution);
            //        await db.SaveChangesAsync();
            //        return RedirectToAction("Details", new { title = request.Title });
            //    }
            //    catch (Exception e)
            //    {
            //        // when will i implement error logging? who knows.
            //    }
            //}

            //return RedirectToAction("Index", "Home");
            #endregion
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { }
            base.Dispose(disposing);
        }
    }
}
