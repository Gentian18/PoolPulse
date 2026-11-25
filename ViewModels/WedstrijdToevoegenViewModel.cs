using System.ComponentModel.DataAnnotations;

namespace Zwemclub.ViewModels
{
    public class WedstrijdToevoegenViewModel
    {
        [Required(ErrorMessage = "Vul de naam van de wedstrijd in.")]
        [MaxLength(256)]
        [Display(Name = "Naam wedstrijd")]
        public string Naam { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul de datum van de wedstrijd in.")]
        [Display(Name = "Datum wedstrijd")]
        public DateOnly? Datum { get; set; }

        [Required(ErrorMessage = "Vul de deadline in.")]
        [Display(Name = "Deadline inschrijving")]
        public DateTime? DeadlineInschrijven { get; set; }

        [Required(ErrorMessage = "Vul de wedstrijdlink in.")]
        [Url(ErrorMessage = "Vul een geldige link in.")]
        [MaxLength(2000)]
        [Display(Name = "Wedstrijdlink")]
        public string UrlWedstrijd { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul de naam van de locatie in.")]
        [MaxLength(256)]
        [Display(Name = "Naam locatie")]
        public string PlaatsNaam { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul de straat in.")]
        [MaxLength(256)]
        [Display(Name = "Straat")]
        public string Straat { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul het huisnummer in.")]
        [MaxLength(256)]
        [Display(Name = "Huisnummer")]
        public string HuisNummer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul de postcode in.")]
        [MaxLength(256)]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul de gemeente in.")]
        [MaxLength(256)]
        [Display(Name = "Gemeente")]
        public string Gemeente { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vul het land in.")]
        [MaxLength(256)]
        [Display(Name = "Land")]
        public string Land { get; set; } = "België";
    }
}