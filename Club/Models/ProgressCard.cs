using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Club.Models
{
    public class ProgressCard
    {
        [Key]
        public int ProgressCardID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime MeasurementDate { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public double BodyFatPercentage { get; set; }
        [Required]
        public double MuscleMass { get; set; }
        [Required]
        public double Height { get; set; }
        [Required]

        // Klucz obcy i właściwość nawigacyjna do klasy Member - asocjacja z kompozycją
        public int MemberID { get; set; }
        public virtual Member Member { get; set; }

        // Atrybut pochodny
        [NotMapped]
        public double BMI => Weight / (Height * Height);
    }
}
