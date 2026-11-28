namespace Zwemclub.ViewModels
{
    public class AccountVerwijderenViewModel
    {
        [Required(ErrorMessage = "Vul je wachtwoord in.")]
        [DataType(DataType.Password)]
        public string Wachtwoord { get; set; } = string.Empty;
    }
}
