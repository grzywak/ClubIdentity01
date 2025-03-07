using Club.Data;
using System.ComponentModel.DataAnnotations;

namespace Club.Models
{
    public class Session
    {

        [Key]
        [Display(Name = "Session ID")]
        public int SessionID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        [Display(Name = "Start Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "End time is required")]
        [Display(Name = "End Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Max participants is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Max participants must be greater than 0")]
        public int MaxParticipants { get; set; }

        [Required(ErrorMessage = "Available slots is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Available slots must be non-negative")]
        public int AvailableSlots { get; set; }

        public int SectionID { get; set; }    
        public Section? Section { get; set; }
        [Required]
        public int TermsAndConditionsID { get; set; }
        public TermsAndConditions? TermsAndConditions { get; set; }
        public virtual ICollection<SessionReservation>? SessionReservations { get; set; }
        public virtual ICollection<CoachAssignment>? CoachAssignments { get; set; }
        public virtual ICollection<Feedback>? Feedbacks { get; set; }

        public virtual ICollection<SessionEquipment>? SessionEquipments { get; set; } = new List<SessionEquipment>();
        


    }
}
