namespace Zwemclub.ViewModels
{
    public class KindWedstrijdViewModel
    {
        public string WedstrijdNaam { get; set; } = string.Empty;

        public DateOnly Datum { get; set; }

        public string ReeksNaam { get; set; } = string.Empty;

        public string Startuur { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? BehaaldeTijd { get; set; }

        public string? BehaaldePlaats { get; set; }
    }
}
