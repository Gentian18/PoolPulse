namespace Zwemclub.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("Voorzitter"))
            {
                return RedirectToAction("Index", "WedstrijdBeheer");
            }

            if (User.IsInRole("Trainer"))
            {
                return RedirectToAction(nameof(Trainer));
            }

            if (User.IsInRole("Ouder"))
            {
                return RedirectToAction(nameof(Ouder));
            }

            if (User.IsInRole("Zwemmer"))
            {
                return RedirectToAction(nameof(Zwemmer));
            }

            return Forbid();
        }

        [Authorize(Roles = "Trainer")]
        public IActionResult Trainer() => View();

        [Authorize(Roles = "Ouder")]
        public IActionResult Ouder() => View();

        [Authorize(Roles = "Zwemmer")]
        public IActionResult Zwemmer() => View();
    }
}