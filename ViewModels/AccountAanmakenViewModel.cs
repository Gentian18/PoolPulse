namespace Zwemclub.ViewModels
{
    public class AccountAanmakenViewModel
    {
        [Required(ErrorMessage = "Een achternaam moet ingevuld worden.")]
        [MaxLength(256)]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Een voornaam moet ingevuld worden")]
        [MaxLength(256)]
        public string  Voornaam { get; set; }

        [Required(ErrorMessage = "Een geboortedatum moet ingevuld worden")]
        [Display(Name = "Geboortedatum")]
        public DateOnly? Geboortedatum { get; set; }

        [Required(ErrorMessage = "Een geslacht moet meegegeven worden.")]
        [Display(Name = "Geslacht")]
        public int? GeslachtId { get; set; }

        [Required(ErrorMessage = "Vul een geldige e-mailadres in.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        [Display(Name = "E-mailadres")]
        public string Emailadres { get; set; }

        [Required(ErrorMessage = "Vul je telefoonnummer in.")]
        [Phone(ErrorMessage = "Vul een geldig telefoonnumer in.")]
        [Display(Name = "Telefoonnummer")]
        public string Telefoonnummer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecteer een rol.")]
        public string Rol { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul een wachtwoord in.")]
        [DataType(DataType.Password)]
        public string Wachtwoord { get; set; } = string.Empty;

        [Required(ErrorMessage = "Herhaal je wachtwoord.")]
        [DataType(DataType.Password)]
        [Display(Name = "Herhaal wachtwoord")]
        [Compare(nameof(Wachtwoord), ErrorMessage = "De wachtwoorden komen niet overeen.")]
        public string BevestigWachtwoord { get; set; } = string.Empty;
    }
}
