using System.ComponentModel.DataAnnotations;

namespace Club.Models.DTOs
{
    public class SessionReservationDTO
    {
        public int MemberID { get; set; }

        public int SessionID { get; set; }

        public DateTime ReservationDate { get; set; } = DateTime.Now;

        public ReservationStatus Status { get; set; } = ReservationStatus.Created;
    }
}
