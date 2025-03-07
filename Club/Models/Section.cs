using Club.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Club.Models
{
    public class Section
    {
        [Key]
        public int SectionID { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Desc { get; set; }

        // public int? CoachID { get; set; }
        //[ForeignKey("SectionID")]
       // public Coach Coach { get; set; }
        public ICollection<Session>? Sessions { get; set; }

    }
}




