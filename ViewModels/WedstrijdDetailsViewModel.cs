namespace Zwemclub.ViewModels
{
    public class WedstrijdDetailsViewModel
    {
        public string Naam { get; set; } = string.Empty;
        public DateOnly Datum { get; set; }
        public string Locatie { get; set; } = string.Empty;
        public string Adres { get; set; } = string.Empty;
        public List<ReeksViewModel> Reeksen { get; set; } = [];
    }

    public class ReeksViewModel
    {
        public string Naam { get; set; } = string.Empty;
        public string Geslacht { get; set; } = string.Empty;
        public string Startuur { get; set; } = string.Empty;
        public List<DeelnemerViewModel> Deelnemers { get; set; } = [];
    }

    public class DeelnemerViewModel
    {
        public string? Voornaam { get; set; }
        public string? Naam { get; set; }
        public string? BehaaldeTijd { get; set; }
        public string? BehaaldePlaats { get; set; }
    }
}
