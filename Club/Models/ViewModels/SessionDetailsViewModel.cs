using Microsoft.AspNetCore.Mvc.Rendering;

namespace Club.Models.ViewModels
{
    public class SessionDetailsViewModel
    {
        public Session Session { get; set; }
        public int SessionID => Session.SessionID;
        public SelectList Members { get; set; }
        public int MemberID { get; set; }
    }
}
