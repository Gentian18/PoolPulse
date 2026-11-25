namespace Zwemclub.ViewModels
{
    public class ContactViewModel
    {
        public string Voornaam { get; set; } = string.Empty;
        public string Naam { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string VolledigeNaam => $"{Voornaam} {Naam}".Trim();
    }
}
