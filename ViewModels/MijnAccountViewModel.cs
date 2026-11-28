namespace Zwemclub.ViewModels
{
    public class MijnAccountViewModel
    {
        [Required(ErrorMessage = "Vul je voornaam in.")]
        [MaxLength(256)]
        public string Voornaam { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul je achternaam in.")]
        [MaxLength(256)]
        [Display(Name = "Achternaam")]
        public string Naam { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul je geboortedatum in.")]
        public DateOnly? Geboortedatum { get; set; }

        [Required(ErrorMessage = "Selecteer een geslacht.")]
        [Display(Name = "Geslacht")]
        public int? GeslachtId { get; set; }

        [Required(ErrorMessage = "Vul je e-mailadres in.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        [MaxLength(256)]
        [Display(Name = "E-mailadres")]
        public string Emailadres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul je telefoonnummer in.")]
        [Phone(ErrorMessage = "Vul een geldig telefoonnummer in.")]
        public string Telefoonnummer { get; set; } = string.Empty;
    }
}
