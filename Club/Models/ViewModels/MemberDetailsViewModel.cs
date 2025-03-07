using Microsoft.AspNetCore.Mvc.Rendering;

namespace Club.Models.ViewModels
{
    public class MemberDetailsViewModel
    {
        public Member Member { get; set; }
        public int SessionID { get; set; }
        public SelectList Sessions { get; set; }
        public ProgressCard ProgressCard { get; set; }

    }
}
