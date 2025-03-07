namespace Club.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Session> LatestSessions { get; set; }
        public IEnumerable<Feedback> LatestFeedbacks { get; set; }

    }
}
