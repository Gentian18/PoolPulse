namespace Zwemclub.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vul je e-mailadres in.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        [Display(Name = "E-mailadres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul je wachtwoord in.")]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Wachtwoord { get; set; } = string.Empty;

        [Display(Name = "Onthoud mij")]
        public bool Onthoud { get; set; }

        public string? ReturnUrl { get; set; }
    }
}