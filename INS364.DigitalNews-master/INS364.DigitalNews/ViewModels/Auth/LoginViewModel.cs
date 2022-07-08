using System.ComponentModel.DataAnnotations;

namespace INS364.DigitalNews.ViewModels.Auth
{
    public class LoginViewModel
    {
        private const string REQUIRED_MSG = "Este campo es requerido.";

        [Required(ErrorMessage = REQUIRED_MSG)]
        public string Username { get; set; }

        [Required(ErrorMessage = REQUIRED_MSG)]
        public string Password { get; set; }
    }
}
