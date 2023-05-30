using RobeazyCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobeazyCore.Services
{
    public interface IFeedbackService
    {
        Feedback CreateBug(FeedbackRequestModel request, ApplicationUser user);
        Feedback CreateSuggestion(FeedbackRequestModel request, ApplicationUser user);
    }
}
