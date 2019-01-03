using System.ComponentModel.DataAnnotations;

namespace WebAppIdentity.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "The password must be at least 5 characters long and contain 4 unique characters.")]
        public string Password { get; set; }
    }
}
