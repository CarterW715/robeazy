namespace RobeazyCore.Models
{
    public interface IStory
    {
        int Id { get; set; }
        Story Story { get; set; }
    }
}
