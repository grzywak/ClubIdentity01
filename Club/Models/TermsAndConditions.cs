using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Club.Models
{
    public class TermsAndConditions
    {
        [Key]
        public int TermsAndConditionsID { get; set; }

        [Required]
        public int SessionID { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }

        [ForeignKey("SessionID")]
        public virtual Session Session { get; set; }
    }
}
