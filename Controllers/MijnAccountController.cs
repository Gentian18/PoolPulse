using Microsoft.AspNetCore.Mvc.Rendering;
using Zwemclub.ViewModels;

namespace Zwemclub.Controllers
{
    [Authorize]
    public class MijnAccountController : Controller
    {
        private readonly UserManager<Gebruiker> _userManager;
        private readonly SignInManager<Gebruiker> _signInManager;
        private readonly ZwemclubContext _context;

        public MijnAccountController(UserManager<Gebruiker> userManager, SignInManager<Gebruiker> signInManager, ZwemclubContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var gebruiker = await _userManager.GetUserAsync(User);
            if (gebruiker == null) return Challenge();
            await VulGeslachtenAsync();
            return View(new MijnAccountViewModel
            {
                Voornaam = gebruiker.Voornaam ?? string.Empty,
                Naam = gebruiker.Naam ?? string.Empty,
                Geboortedatum = gebruiker.Geboortedatum,
                GeslachtId = gebruiker.GeslachtId,
                Emailadres = gebruiker.Email ?? string.Empty,
                Telefoonnummer = gebruiker.PhoneNumber ?? string.Empty
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(MijnAccountViewModel model)
        {
            var gebruiker = await _userManager.GetUserAsync(User);
            if (gebruiker == null) return Challenge();
            if (model.Geboortedatum > DateOnly.FromDateTime(DateTime.Today))
                ModelState.AddModelError(nameof(model.Geboortedatum), "De geboortedatum mag niet in de toekomst liggen.");
            if (model.GeslachtId.HasValue && !await _context.Geslachten.AnyAsync(g => g.Id == model.GeslachtId && !g.IsVerwijderd))
                ModelState.AddModelError(nameof(model.GeslachtId), "Selecteer een geldig geslacht.");
            if (ModelState.IsValid)
            {
                var bestaandAccount = await _userManager.FindByEmailAsync(model.Emailadres);
                if (bestaandAccount != null && bestaandAccount.Id != gebruiker.Id)
                    ModelState.AddModelError(nameof(model.Emailadres), "Dit e-mailadres is al in gebruik.");
            }
            if (!ModelState.IsValid)
            {
                await VulGeslachtenAsync();
                return View(model);
            }

            gebruiker.Voornaam = model.Voornaam;
            gebruiker.Naam = model.Naam;
            gebruiker.Geboortedatum = model.Geboortedatum;
            gebruiker.GeslachtId = model.GeslachtId;
            gebruiker.Email = model.Emailadres;
            gebruiker.UserName = model.Emailadres;
            gebruiker.PhoneNumber = model.Telefoonnummer;
            var resultaat = await _userManager.UpdateAsync(gebruiker);
            if (!resultaat.Succeeded)
            {
                VoegFoutenToe(resultaat);
                await VulGeslachtenAsync();
                return View(model);
            }
            await _signInManager.RefreshSignInAsync(gebruiker);
            TempData["Melding"] = "Je gegevens zijn opgeslagen.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Verwijderen() => View(new AccountVerwijderenViewModel());

        [HttpPost]
        public async Task<IActionResult> Verwijderen(AccountVerwijderenViewModel model)
        {
            var gebruiker = await _userManager.GetUserAsync(User);
            if (gebruiker == null) return Challenge();
            if (!ModelState.IsValid) return View(model);
            var controle = await _signInManager.CheckPasswordSignInAsync(gebruiker, model.Wachtwoord, lockoutOnFailure: true);
            if (!controle.Succeeded)
            {
                ModelState.AddModelError(string.Empty, controle.IsLockedOut
                    ? "Dit account is tijdelijk geblokkeerd. Probeer later opnieuw."
                    : "Het wachtwoord is onjuist of het account is niet beschikbaar.");
                return View(model);
            }
            gebruiker.IsActief = false;
            gebruiker.IsVerwijderd = true;
            gebruiker.SecurityStamp = Guid.NewGuid().ToString();
            var resultaat = await _userManager.UpdateAsync(gebruiker);
            if (!resultaat.Succeeded)
            {
                VoegFoutenToe(resultaat);
                return View(model);
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task VulGeslachtenAsync()
        {
            var geslachten = await _context.Geslachten.Where(g => !g.IsVerwijderd).OrderBy(g => g.Naam).ToListAsync();
            ViewBag.Geslachten = new SelectList(geslachten, nameof(Geslacht.Id), nameof(Geslacht.Naam));
        }

        private void VoegFoutenToe(IdentityResult resultaat)
        {
            foreach (var fout in resultaat.Errors)
                ModelState.AddModelError(string.Empty, fout.Description);
        }
    }
}
