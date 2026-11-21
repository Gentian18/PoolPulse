namespace Zwemclub.Controllers
{
    [AllowAnonymous]
    public class FotosController : Controller
    {
        public IActionResult Index()
        {
            return View("Binnenkort", "Foto’s");
        }
    }
}
