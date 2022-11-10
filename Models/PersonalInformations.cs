using System.ComponentModel.DataAnnotations;

namespace Test.Models
{
    public class PersonalInformations
    {
        public Guid Id { get; set; } //Id первый первичный ключ 


        [Required(ErrorMessage = "Fill login")]
        [Display(Name = "User login")]
        public string Login { get; set; }

        [RegularExpression(@"^[a-zA-Z]\w{3,14}$")]
        [StringLength(30)]
        [Required]
        public string Password { get; set; }

        [StringLength(60)]
        [Required]
        //[Required(ErrorMessage = "Fill the form")]
        //[Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Fill the form")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Fill Gender")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        
        [Display(Name = "Year Of Birth")]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime YearOfBirth { get; set; }
    }
}
