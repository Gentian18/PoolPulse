using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Zwemclub.ViewModels;

namespace Zwemclub.Controllers
{
    public class AccountAanmakenController : Controller
    {
        private readonly ZwemclubContext _context;

        // Services
        private readonly UserManager<Gebruiker> _userManager;

        private readonly SignInManager<Gebruiker> _signInManager;

        public AccountAanmakenController(ZwemclubContext context, UserManager<Gebruiker> userManager,
            SignInManager<Gebruiker> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await VulGeslachtenAsync();

            return View(new AccountAanmakenViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(AccountAanmakenViewModel vm)
        {
            if (vm.Rol != "Zwemmer" && vm.Rol != "Ouder")
            {
                ModelState.AddModelError(
                    nameof(vm.Rol),
                    "Selecteer een geldige rol");
            }

            if (vm.Geboortedatum.HasValue &&
                vm.Geboortedatum.Value >
                DateOnly.FromDateTime(DateTime.Today))
            {
                ModelState.AddModelError(
                    nameof(vm.Geboortedatum),
                    "De geboortedatum mag niet in de toekomst plaatsvinden.");
            }

            if (vm.GeslachtId.HasValue)
            {
                bool bestaat = await _context.Geslachten
                    .AnyAsync(x => x.Id == vm.GeslachtId && !x.IsVerwijderd);

                if (!bestaat)
                {
                    ModelState.AddModelError(
                        nameof(vm.GeslachtId),
                        "Selecteer een geldig geslacht.");
                }
            }

                if (!ModelState.IsValid)
                {
                    await VulGeslachtenAsync();
                    return View(vm);
                }

                var bestaandAccount = await
                    _userManager.FindByEmailAsync(vm.Emailadres);

                if (bestaandAccount != null)
                {
                    ModelState.AddModelError(
                        nameof(vm.Emailadres),
                        "Dit e-mailadres is al in gebruik.");

                    await VulGeslachtenAsync();
                    return View(vm);
                }

                var gebruiker = new Gebruiker
                {
                    Voornaam = vm.Voornaam,
                    Naam = vm.Naam,
                    Geboortedatum = vm.Geboortedatum,
                    GeslachtId = vm.GeslachtId,
                    Email = vm.Emailadres,
                    UserName = vm.Emailadres,
                    PhoneNumber = vm.Telefoonnummer,
                    UniekNummer = Guid.NewGuid().ToString("N"),
                    IsActief = true
                };

                await using var transactie = await
                    _context.Database.BeginTransactionAsync();

                var resultaat = await
                    _userManager.CreateAsync(
                        gebruiker,
                        vm.Wachtwoord);

                if (!resultaat.Succeeded)
                {
                    await transactie.RollbackAsync();
                    VoegIdentityFoutenToe(resultaat);
                    await VulGeslachtenAsync();
                    return View(vm);
                }

                var rolResultaat = await
                    _userManager.AddToRoleAsync(
                        gebruiker, vm.Rol);

                if (!rolResultaat.Succeeded)
                {
                    await transactie.RollbackAsync();
                    VoegIdentityFoutenToe(rolResultaat);
                    await VulGeslachtenAsync();
                    return View(vm);
                }

                await transactie.CommitAsync();

                await _signInManager.SignInAsync(
                    gebruiker,
                    isPersistent: false);

                TempData["AccountAangemaakt"] = true;
                return RedirectToAction(nameof(Bevestiging));
        }

        [HttpGet]
        public async Task<IActionResult> Bevestiging()
        {
            if (TempData["AccountAangemaakt"] is not true)
            {
                return RedirectToAction(nameof(Index));
            }

            var gebruiker = await _userManager.GetUserAsync(User);
            if (gebruiker == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var geslacht = await _context.Geslachten
                .Where(x => x.Id == gebruiker.GeslachtId)
                .Select(x => x.Naam)
                .FirstOrDefaultAsync();
            var rollen = await _userManager.GetRolesAsync(gebruiker);

            return View(new AccountBevestigingViewModel
            {
                Voornaam = gebruiker.Voornaam,
                Naam = gebruiker.Naam,
                Geboortedatum = gebruiker.Geboortedatum,
                Geslacht = geslacht,
                Emailadres = gebruiker.Email,
                Telefoonnummer = gebruiker.PhoneNumber,
                Rol = string.Join(", ", rollen)
            });
        }

        private async Task VulGeslachtenAsync()
        {
            var geslachten = await _context.Geslachten
                .Where(x => !x.IsVerwijderd)
                .OrderBy(x => x.Naam)
                .ToListAsync();

            ViewBag.Geslachten = new SelectList(
                geslachten, nameof(Geslacht.Id),
                nameof(Geslacht.Naam));
        }

        private void VoegIdentityFoutenToe(IdentityResult result)
        {
            foreach (var fout in result.Errors)
            {
                ModelState.AddModelError(string.Empty, fout.Description);
            }
        }
    }
}
