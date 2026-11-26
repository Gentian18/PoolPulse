namespace Zwemclub.ViewModels.Kinderen
{
    public class KindToevoegenViewModel
    {
        [Required(ErrorMessage = "Voornaam is verplicht.")]
        public string Voornaam { get; set; } = string.Empty;

        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Naam { get; set; } = string.Empty;

        [Required(ErrorMessage = "Geboortedatum is verplicht.")]
        public DateOnly? Geboortedatum { get; set; }

        [Required(ErrorMessage = "Geslacht is verplicht.")]
        public int? GeslachtId { get; set; }
    }
}
