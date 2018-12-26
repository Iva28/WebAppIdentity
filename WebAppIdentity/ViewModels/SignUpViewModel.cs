using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppIdentity.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "FullName is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "The password must be at least 5 characters long and contain 4 unique characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "BirthDate is required.")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }
}
