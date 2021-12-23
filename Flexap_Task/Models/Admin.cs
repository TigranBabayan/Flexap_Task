using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flexap_Task.Models

{
    public class Admin
    {
        public int Id { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Please enter valid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter valid Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The Password didn't match")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string ConfirmPassword { get; set; }
    }
}
