using RobeazyCore.Data;
using RobeazyCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobeazyCore.Respository
{
    public class ContributionRepository : IContributionRepository
    {
        private readonly ApplicationDbContext _context;

        public ContributionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Contribution GetContributionById(int contributionId)
            => _context.Contributions.Find(contributionId);

        public Contribution IncrementContributionLikes(Contribution contribution)
        {
            _context.Contributions.Attach(contribution);
            contribution.Upvotes++;
            _context.SaveChangesAsync();

            return contribution;
        }

        public Contribution DecrementContributionLikes(Contribution contribution)
        {
            _context.Contributions.Attach(contribution);
            if (contribution.Upvotes > 0)
            {
                contribution.Upvotes--;
            }
            _context.SaveChangesAsync();

            return contribution;
        }
    }
}