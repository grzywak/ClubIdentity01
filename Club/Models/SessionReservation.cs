using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Club.Models
{
    public class SessionReservation
    {
      
            [Key]
            public int SessionReservationID { get; set; }
            public int SessionID { get; set; }
            [ForeignKey("SessionID")]
            public Session? Session { get; set; }
            public int MemberID { get; set; }
            [ForeignKey("MemberID")]
            public Member? Member { get; set; }

            [Required]    
            [Display(Name = "Reservation Date")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
            public DateTime ReservationDate { get; set; }
            [Required]
            [Display(Name = "Status")]         
            public ReservationStatus Status { get; set; }
    }

    public enum ReservationStatus
    {
        [Display(Name = "Created")]
        Created,

        [Display(Name = "Confirmed")]
        Confirmed,

        [Display(Name = "Cancelled")]
        Cancelled,

        [Display(Name = "Completed")]
        Completed
    }
}
