namespace INS364.DigitalNews.Utility
{
    public abstract class ErrorMessages
    {
        public const string REQUIRED_MSG = "Este campo es requerido.";
        public const string LESS_MIN_MSG = "La contraseña debe tener al menos 8 caracteres.";
        public const string PWD_CHR_MSG = "La contraseña debe contener al menos un caracter en mayúscula, otro en minúscula y un dígito.";
        public const string INVALID_EMAIL_MSG = "El correo electrónico es invalido.";
    }
}