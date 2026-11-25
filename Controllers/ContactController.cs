using Zwemclub.ViewModels;

namespace Zwemclub.Controllers
{
    [AllowAnonymous]
    public class ContactController : Controller
    {
        private readonly UserManager<Gebruiker> _userManager;

        public ContactController(UserManager<Gebruiker> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var voorzitter =
                await _userManager.FindByIdAsync("zwemclub-voorzitter");

            if (voorzitter == null)
            {
                return View(new ContactViewModel());
            }

            var viewModel = new ContactViewModel
            {
                Voornaam = voorzitter.Voornaam ?? string.Empty,
                Naam = voorzitter.Naam ?? string.Empty,
                Email = voorzitter.Email ?? string.Empty
            };

            return View(viewModel);
        }
    }
}
