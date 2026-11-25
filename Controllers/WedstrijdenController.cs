namespace Zwemclub.Controllers
{
    [AllowAnonymous]
    public class WedstrijdenController(IUnitOfWork unitOfWork) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await unitOfWork.Wedstrijden.GetPubliekOverzichtAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var wedstrijd = await unitOfWork.Wedstrijden.GetPubliekeDetailsAsync(id);
            return wedstrijd == null ? NotFound() : View(wedstrijd);
        }
    }
}
