using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Club.Models
{
    public class CoachAssignment
    {
        [Key]
        public int CoachAssignmentID { get; set; }

        [Required]
        public int CoachID { get; set; }

        [ForeignKey("CoachID")]
        public Coach? Coach { get; set; }

        [Required]
        public int SessionID { get; set; }

        [ForeignKey("SessionID")]
        public Session? Session { get; set; }

        [Required(ErrorMessage = "Compensation is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Compensation { get; set; }

        [Required(ErrorMessage = "IsLeadCoach is required")]
        public bool IsLeadCoach { get; set; }

    }
}
