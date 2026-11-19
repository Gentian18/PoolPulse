namespace Zwemclub.Models
{
    public class Parameter:IModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Naam { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string Waarde { get; set; } = string.Empty;
    }
}
