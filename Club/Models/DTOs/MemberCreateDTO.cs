using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Club.Models.DTOs
{
    public class MemberCreateDTO
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Registered Date")]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        public ProgressCardDTO ProgressCard { get; set; }
    }

    public class ProgressCardDTO
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime MeasurementDate { get; set; }

        [Required]
        [Range(1, 500)]
        public double Weight { get; set; }

        [Required]
        [Range(1, 100)]
        public double BodyFatPercentage { get; set; }

        [Required]
        [Range(1, 100)]
        public double MuscleMass { get; set; }

        [Required]
        [Range(100, 300)]
        public double Height { get; set; }
    }
}
