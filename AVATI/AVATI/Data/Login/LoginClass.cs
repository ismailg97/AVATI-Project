using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AVATI.Data
{
    public class LoginClass
    {
        [NotNull]
        [Required(ErrorMessage = "Bitte geben Sie ein Benutzernamen an")]
        [StringLength(70, ErrorMessage = "Benutzername ist zu lang (70 Zeichen)")]
        public string Username { get; set; }
        
        [NotNull]
        [Required(ErrorMessage = "Bitte geben Sie einen Passwort an")]
        [StringLength(70, ErrorMessage = "Passwort ist zu lang (70 Zeichen)")]
        public string Password { get; set; }
    }
}