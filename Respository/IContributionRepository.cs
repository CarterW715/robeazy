using RobeazyCore.Models;
using System.Collections.Generic;

namespace RobeazyCore.Respository
{
    public interface IContributionRepository
    {
        Contribution GetContributionById(int contributionId);
        Contribution IncrementContributionLikes(Contribution contribution);
        Contribution DecrementContributionLikes(Contribution contribution);
    }
}
