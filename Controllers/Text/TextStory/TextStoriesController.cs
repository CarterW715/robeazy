using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RobeazyCore.Models;
using RobeazyCore.Services;

namespace RobeazyCore.Controllers
{
    public class TextStoriesController : Controller
    {
        private readonly IStoryService<TextStory, TextStoryViewModel, TextStoryContribution, TextStoryContributionView> _storyService;
        private readonly IUserService _userService;

        public TextStoriesController(IStoryService<TextStory, TextStoryViewModel, TextStoryContribution, TextStoryContributionView> storyService, IUserService userService)
        {
            _storyService = storyService;
            _userService = userService;
        }

        // GET: Text
        public ActionResult Index()
        {
            return View();
        }

        // GET
        public ActionResult Details(string title, string[] contributionIds)
        {
            var storyResults = _storyService.GetStoryDetails(title, _userService.User, contributionIds);
            var view = storyResults.Story;
            view.ContributionViewModels = storyResults.ContributionViews;
            return View(view);
            #region old code
            //Story story = db.Stories.FirstOrDefault(s => s.Title == title);
            //if (story == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //TextStory text = db.TextStories.FirstOrDefault(x => x.Story.StoryId == story.StoryId);
            //if (text == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            //int HighlightId = 0;

            //List<TextStoryContributionView> ContributionViewModels = new List<TextStoryContributionView>();

            //TextStoryQueries query = new TextStoryQueries(db);

            //if (contributionIds == null || contributionIds.Length == 0)
            //{
            //    TextStoryContributionView ContributionViewModel = query.GetFirstContribution(text.Story, user);

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
            //    TextStoryContribution Contribution = db.TextStoryContributions.Find(id);
            //    if (Contribution == null)
            //    {
            //        return Json(new { success = false, responseText = "Contribution was not found" }, JsonRequestBehavior.DenyGet);
            //    }

            //    TextStoryContributionView ContributionViewModel = TextStoryContributionView.ContributionToViewModel(Contribution, db, user);
            //    HighlightId = id;
            //    ContributionViewModels.Add(ContributionViewModel);

            //    // Get up the chain all of the way to the root of the story
            //    TextStoryContribution PreviousContribution = Contribution.PreviousContribution;      
            //    while (PreviousContribution != null)
            //    {
            //        TextStoryContributionView PreviuousContributionViewModel = TextStoryContributionView.ContributionToViewModel(PreviousContribution, db, user);
            //        ContributionViewModels.Insert(0, PreviuousContributionViewModel);
            //        PreviousContribution = PreviousContribution.PreviousContribution; // Go up the chain
            //    }

            //    // Now add the other inputed contributions
            //    for (int count = 1; count < contributionIds.Length; count++)
            //    {
            //        id = Int32.Parse(contributionIds[count]);
            //        Contribution = db.TextStoryContributions.Find(id);
            //        if (Contribution == null)
            //        {
            //            return Json(new { success = false, responseText = "Contribution was not found" }, JsonRequestBehavior.DenyGet);
            //        }

            //        ContributionViewModel = TextStoryContributionView.ContributionToViewModel(Contribution, db, user);
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

            //TextStoryViewModel viewStory = new TextStoryViewModel
            //{
            //    Story = story,
            //    Text = text,
            //    HighlightId = HighlightId,
            //    ContributionViewModels = ContributionViewModels,
            //    Liked = userLike != null,
            //    Reported = userReport != null
            //};
            #endregion
        }

        // GET: Text/Create
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CreateTextStoryModel();
            return View(viewModel);
        }

        // POST: Text/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTextStoryModel textRequest)
        {
            if (_userService.User == null)
            {
                return Json(new { success = false, responseText = "User is not logged in" });
            }

            var storyResults = _storyService.CreateStory(textRequest, _userService.User);

            if (!storyResults.Success)
            {
                return Json(new { success = false, errors = storyResults.GetErrors() });
            }

            return RedirectToAction("Details", new { title = textRequest.Title });

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
            //        if (!textRequest.IsWordConstraintValid())
            //        {
            //            string errorMsg = String.Format("The maximum word count cannot be lower than the minimum word count", textRequest.WordMin, textRequest.WordMax);
            //            ModelState.AddModelError(string.Empty, errorMsg);
            //        }
            //        WordConstraintResult result = ContributionRules.IsContributionWordCountValid(textRequest.WordMin, textRequest.WordMax, textRequest.FirstContribution);
            //        if (!result.Success)
            //        {
            //            string errorMsg = String.Format("Your contribution was {0} words and must be between {1} and {2} words in length", result.Count, textRequest.WordMin, textRequest.WordMax);
            //            ModelState.AddModelError(string.Empty, errorMsg);
            //        }
            //        else
            //        {
            //            try
            //            {

            //                //Story newStory = new Story(text.Title, user, StoryType.Text);

            //                // Create the story for the database with the given values from the view
            //                TextStory newText = new TextStory(textRequest.Title, user, textRequest.WordMax, textRequest.DynamicContributions, textRequest.Genre);

            //                // Probably needs to be in a separate controller/partial view...
            //                TextStoryContribution firstContribution = new TextStoryContribution(user, newText.Story, textRequest.FirstContribution);

            //                // Persist to database
            //                db.TextStories.Add(newText);
            //                db.TextStoryContributions.Add(firstContribution);
            //                db.SaveChanges();
            //                return RedirectToAction("Details", new { title = textRequest.Title });
            //            }
            //            catch (Exception e)
            //            {
            //                var test = e.Message;
            //                // do something
            //            }
            //        }
            //    }
            //}

            //return RedirectToAction("Index", "Home");
            #endregion
        }

        // GET: Text/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TextStory text = _dB.TextStories.Find(id);
        //    if (text == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(text);
        //}

        //// POST: Text/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "TextId,Title,Upvotes,Downvotes,StoryType,CreateDate,TextId,Genre")] TextStory text)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _dB.Entry(text).State = EntityState.Modified;
        //        _dB.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(text);
        //}

        //// GET: Text/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TextStory text = _dB.TextStories.Find(id);
        //    if (text == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(text);
        //}

        //// POST: Text/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    TextStory text = _dB.TextStories.Find(id);
        //    _dB.TextStories.Remove(text);
        //    _dB.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_dB.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
