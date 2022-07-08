using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace INS364.DigitalNews.Utility
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
            {
                // Since a news without audivisual material can be uploaded, a validation result
                // denoting a success is returned. Otherwise, this should return a new validation 
                // result with a custom error message.
                return ValidationResult.Success;
            }

            var file = value as IFormFile;
            var extension = Path.GetExtension(file.FileName);
            if (file != null)
            {
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(errorMessage:
                        "La imagen contiene una extensión inválida. " +
                        $"Use una de estas: {string.Join(",", _extensions)}");
                }
            }

            return ValidationResult.Success;
        }
    }
}