namespace Zwemclub.ViewModels
{
    public class KindViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string Voornaam { get; set; } = string.Empty;

        public string Naam { get; set; } = string.Empty;

        public DateOnly? Geboortedatum { get; set; }

        public string UniekNummer { get; set; } = string.Empty;

        public string? ProfielFoto { get; set; }

        public List<KindWedstrijdViewModel> Wedstrijden { get; set; } = [];
    }
}
