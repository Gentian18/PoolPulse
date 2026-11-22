namespace Zwemclub.Controllers
{
    [AllowAnonymous]
    public class GebruikerController : Controller
    {
        public IActionResult Registreren()
        {
            return View("Binnenkort", "Inschrijven");
        }

        public IActionResult Login()
        {
            return View("Binnenkort", "Aanmelden");
        }
    }
}
