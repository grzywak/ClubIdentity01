using Microsoft.AspNetCore.Mvc.Rendering;

namespace Club.Models.ViewModels
{
    public class RegisterForSessionViewModel
    {

            public int MemberId { get; set; }
            public int SessionId { get; set; }
            public SelectList Sessions { get; set; }

    }
}
