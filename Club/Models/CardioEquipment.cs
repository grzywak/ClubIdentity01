using System.ComponentModel.DataAnnotations;

namespace Club.Models
{
    public class CardioEquipment : Equipment
    {
        [Required]
        public int MaxSpeed { get; set; }
        [Required]
        public int MaxIncline { get; set; }
    }
}
