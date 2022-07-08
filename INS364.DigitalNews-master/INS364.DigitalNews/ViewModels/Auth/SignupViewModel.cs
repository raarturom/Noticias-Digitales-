using INS364.DigitalNews.Utility;
using System.ComponentModel.DataAnnotations;

namespace INS364.DigitalNews.ViewModels.Auth
{
    public class SignupViewModel : ErrorMessages
    {
        [Required(ErrorMessage = REQUIRED_MSG)]
        public string Firstname { get; set; }

        [Required(ErrorMessage = REQUIRED_MSG)]
        public string Lastname { get; set; }

        [Required(ErrorMessage = REQUIRED_MSG)]
        [EmailAddress(ErrorMessage = INVALID_EMAIL_MSG)]
        public string Email { get; set; }

        [Required(ErrorMessage = REQUIRED_MSG)]
        public string Username { get; set; }

        [Required(ErrorMessage = REQUIRED_MSG)]
        [MinLength(8, ErrorMessage = LESS_MIN_MSG)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = PWD_CHR_MSG)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}