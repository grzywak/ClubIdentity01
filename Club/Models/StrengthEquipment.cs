using System.ComponentModel.DataAnnotations;

namespace Club.Models
{
    public class StrengthEquipment : Equipment
    {
        [Required]
        public int MaxWeight { get; set; }
        [Required]
        public string MuscleGroup { get; set; }
    }
}
