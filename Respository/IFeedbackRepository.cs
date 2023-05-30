using RobeazyCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobeazyCore.Respository
{
    public interface IFeedbackRepository
    {
        void AddFeedback(Feedback feedback);
    }
}
