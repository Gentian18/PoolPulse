namespace Zwemclub.Models
{
    public class VoogdZwemmer:IModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        public string VoogdId { get; set; } = string.Empty;

        [Required]
        [MaxLength(450)]
        public string ZwemmerId { get; set; } = string.Empty;

        public bool? IsMailGestuurd { get; set; }

        public Gebruiker Voogd { get; set; } = null!;

        public Gebruiker Zwemmer { get; set; } = null!;
    }
}
