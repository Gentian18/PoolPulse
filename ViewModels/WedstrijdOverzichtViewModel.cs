namespace Zwemclub.ViewModels
{
    public class WedstrijdOverzichtViewModel
    {
        public int Id { get; set; }

        public string Naam { get; set; } = string.Empty;

        public DateOnly Datum { get; set; }

        public string Locatie { get; set; } = string.Empty;
    }
}