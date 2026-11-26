namespace Zwemclub.ViewModels
{
    public class KindKoppelenViewModel
    {
        [Required(ErrorMessage = "Uniek nummer is verplicht.")]
        public string UniekNummer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Geboortedatum is verplicht.")]
        public DateOnly Geboortedatum { get; set; }
    }
}
