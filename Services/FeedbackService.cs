using RobeazyCore.Models;
using RobeazyCore.Respository;

namespace RobeazyCore.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepo;

        public FeedbackService(IFeedbackRepository feedbackRepo)
        {
            _feedbackRepo = feedbackRepo;
        }

        public Feedback CreateBug(FeedbackRequestModel request, ApplicationUser user)
        {
            string feedback = request.Feedback;
            if (feedback == null || feedback.Length == 0)
            {
                return null;
            }

            var newFeedback = new Feedback(FeedbackType.Bug, user, feedback);

            _feedbackRepo.AddFeedback(newFeedback);

            return newFeedback;
        }

        public Feedback CreateSuggestion(FeedbackRequestModel request, ApplicationUser user)
        {
            string feedback = request.Feedback;
            if (feedback == null || feedback.Length == 0)
            {
                return null;
            }

            var newFeedback = new Feedback(FeedbackType.Bug, user, feedback);

            _feedbackRepo.AddFeedback(newFeedback);

            return newFeedback;
        }
    }
}
