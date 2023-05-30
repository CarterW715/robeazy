using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace RobeazyCore.Models
{
    public enum MovieElement
    {
        Action, SceneHeading, CharacterName, Extension, Dialogue, Parenthetical, Transition, Shot, DualDialogue
    }

    public class MovieScriptContribution
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public MovieElement Element { get; set; }

        [Required]
        public int ElementOrder { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int ContributionId { get; set; }
        public Contribution Contribution { get; set; }

        public int? PreviousContributionId { get; set; }
        public virtual Contribution PreviousContribution { get; set; }

        public MovieScriptContribution(Contribution contribution, MovieElement element, string content, int elementOrder)
        {
            Element = element;
            Content = content;
            ElementOrder = elementOrder;
            Contribution = contribution;
        }

        public MovieScriptContribution(Contribution contribution, Contribution previous, MovieElement element, string content, int elementOrder)
        {
            Element = element;
            Content = content;
            ElementOrder = elementOrder;
            PreviousContribution = previous;
            Contribution = contribution;
        }

        public MovieScriptContribution() { }

        public static List<MovieScriptContribution> CreateAggregateContribution(ApplicationUser user, Story story, List<MovieScriptContributionCreate> elements)
        {
            Contribution contribution = new Contribution(user, story);
            return elements.Select(e => new MovieScriptContribution(contribution, e.Element, e.Content, e.Order)).ToList();
        }
    }

    public class MovieScriptContributionAggregate : IContribution
    {

        public int Id { get; set; }
        public virtual Contribution Contribution { get; set; }
        public virtual Contribution PreviousContribution { get; set; }
        public List<MovieScriptContribution> Elements { get; set; }

        public MovieScriptContributionAggregate(List<MovieScriptContribution> elements)
        {
            Contribution = elements.FirstOrDefault()?.Contribution;
            Elements = elements.Select(e => new MovieScriptContribution(Contribution, e.Element, e.Content, e.ElementOrder)).ToList();
            Id = Elements == null || Elements.Count == 0 ? 0 : Elements.First().Id;
        }

        public MovieScriptContributionAggregate(ApplicationUser user, Story story, List<MovieScriptContributionCreate> elements)
        {
            Contribution = new Contribution(user, story);
            Elements = elements.Select(e => new MovieScriptContribution(Contribution, e.Element, e.Content, e.Order)).ToList();
            Id = Elements == null || Elements.Count == 0 ? 0 : Elements.First().Id;
        }

        public MovieScriptContributionAggregate(ApplicationUser user, Story story, List<MovieScriptContribution> elements)
        {
            Contribution = new Contribution(user, story);
            Elements = elements;
            Id = Elements == null || Elements.Count == 0 ? 0 : Elements.First().Id;
        }

        public MovieScriptContributionAggregate(ApplicationUser user, Story story, MovieScriptContributionAggregate previousContribution , List<MovieScriptContributionCreate> elements)
        {
            Contribution = new Contribution(user, story);
            PreviousContribution = previousContribution.Contribution;
            Elements = elements.Select(e => new MovieScriptContribution(Contribution, previousContribution.Contribution, e.Element, e.Content, e.Order)).ToList();
            Id = Elements == null || Elements.Count == 0 ? 0 : Elements.First().Id;
        }

        public MovieScriptContributionAggregate(ApplicationUser user, Story story, Contribution previousContribution, List<MovieScriptContributionCreate> elements)
        {
            Contribution = new Contribution(user, story);
            PreviousContribution = previousContribution;
            Elements = elements.Select(e => new MovieScriptContribution(Contribution, previousContribution, e.Element, e.Content, e.Order)).ToList();
            Id = Elements == null || Elements.Count == 0 ? 0 : Elements.First().Id;
        }
    }
}