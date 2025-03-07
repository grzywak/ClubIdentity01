using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Club.Models
{
    public class SessionEquipment
    {
        [Key]
        public int SessionEquipmentID { get; set; }

        [Required]
        public int SessionID { get; set; }
        [ForeignKey("SessionID")]
        public Session? Session { get; set; }

        [Required]
        public int EquipmentID { get; set; }
        [ForeignKey("EquipmentID")]
        public Equipment? Equipment { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
    }
}
