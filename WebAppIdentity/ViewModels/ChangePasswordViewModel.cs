using System.ComponentModel.DataAnnotations;

namespace WebAppIdentity.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "The password must be at least 5 characters long and contain 4 unique characters.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "The password must be at least 5 characters long and contain 4 unique characters.")]
        public string NewPassword { get; set; }
    }
}
