namespace RobeazyCore.Models
{
    public class ReportRequestModel
    {
        public int EntityId { get; set; }
        public string Reason { get; set; }
    }

    public class FeedbackRequestModel
    {
        public string Feedback { get; set; }
    }
}