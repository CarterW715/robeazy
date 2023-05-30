namespace RobeazyCore.Models
{
    public interface IContribution
    {
        int Id { get; set; }
        Contribution Contribution { get; set; }
        Contribution PreviousContribution { get; set; }
    }
}
