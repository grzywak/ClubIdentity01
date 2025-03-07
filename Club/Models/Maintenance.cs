using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Club.Models
{
    public class Maintenance
    {
        [Key]
        public int MaintenanceID { get; set; }

        [Required]
        [ForeignKey("Equipment")]
        public int EquipmentID { get; set; }

        [Required]
        public Equipment Equipment { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Maintenance Date")]
        public DateTime MaintenanceDate { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }
    }
}
