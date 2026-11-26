using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zwemclub.Models;
using Zwemclub.ViewModels;
using Zwemclub.ViewModels.Kinderen;

namespace Zwemclub.Controllers
{
    [Authorize(Roles = "Ouder")]
    public class KinderenController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Gebruiker> _userManager;


        public KinderenController(IUnitOfWork unitOfWork, UserManager<Gebruiker> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        private async Task VulGeslachtenAsync()
        {
            List<Geslacht> geslachten =
                await _unitOfWork.Geslachten.GetAllAsync();

            ViewBag.Geslachten = new SelectList(
                geslachten,
                nameof(Geslacht.Id),
                nameof(Geslacht.Naam));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Gebruiker? ouder = await _userManager.GetUserAsync(User);

            if (ouder == null)
            {
                return Unauthorized();
            }

            List<VoogdZwemmer> gekoppeldeKinderen =
                await _unitOfWork.VoogdZwemmers.GetAllAsync();

            List<Gebruiker> gebruikers =
                await _unitOfWork.GebruikerRepository.GetAllAsync();

            IEnumerable<Gebruiker> kinderen = gebruikers.Where(g =>
                gekoppeldeKinderen.Any(k =>
                    k.VoogdId == ouder.Id &&
                    k.ZwemmerId == g.Id));

            KinderenViewModel vm = new KinderenViewModel
            {
                Kinderen = kinderen.Adapt<List<KindViewModel>>()
            };

            List<InschrijvingWedstrijd> inschrijvingen =
       await _unitOfWork.InschrijvingWedstrijden.GetAllAsync();

            List<WedstrijdReeks> reeksen =
                await _unitOfWork.WedstrijdReeksen.GetAllAsync();

            List<Wedstrijd> wedstrijden =
                await _unitOfWork.Wedstrijden.GetAllAsync();

            List<ReeksType> reeksTypes =
                await _unitOfWork.ReeksTypes.GetAllAsync();

            foreach (KindViewModel kind in vm.Kinderen)
            {
                List<InschrijvingWedstrijd> inschrijvingenKind =
                    inschrijvingen
                        .Where(i =>
                            i.ZwemmerId == kind.Id &&
                            !i.IsVerwijderd)
                        .ToList();

                foreach (InschrijvingWedstrijd inschrijving in inschrijvingenKind)
                {
                    WedstrijdReeks? reeks = reeksen.FirstOrDefault(r =>
                        r.Id == inschrijving.WedstrijdReeksId);

                    if (reeks == null)
                    {
                        continue;
                    }

                    Wedstrijd? wedstrijd = wedstrijden.FirstOrDefault(w =>
                        w.Id == reeks.WedstrijdId);

                    ReeksType? reeksType = reeksTypes.FirstOrDefault(r =>
                        r.Id == reeks.ReeksTypeId);

                    if (wedstrijd == null || reeksType == null)
                    {
                        continue;
                    }

                    string status;

                    if (inschrijving.DatumAnnulatie != null)
                    {
                        status = "Geannuleerd";
                    }
                    else if (inschrijving.DatumDefinitief != null)
                    {
                        status = "Definitief";
                    }
                    else
                    {
                        status = "Ingeschreven";
                    }

                    KindWedstrijdViewModel wedstrijdVm =
                        new KindWedstrijdViewModel
                        {
                            WedstrijdNaam = wedstrijd.Naam,
                            Datum = wedstrijd.Datum,
                            ReeksNaam = reeksType.Naam,
                            Startuur = reeks.Startuur,
                            Status = status,
                            BehaaldeTijd = inschrijving.BehaaldeTijd,
                            BehaaldePlaats = inschrijving.BehaaldePlaats
                        };

                    kind.Wedstrijden.Add(wedstrijdVm);
                }
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Koppelen()
        {
            KindKoppelenViewModel vm = new KindKoppelenViewModel();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Koppelen(KindKoppelenViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            Gebruiker? ouder = await _userManager.GetUserAsync(User);

            if (ouder == null)
            {
                return Unauthorized();
            }

            List<Gebruiker> gebruikers =
                await _unitOfWork.GebruikerRepository.GetAllAsync();

            Gebruiker? zwemmer = gebruikers.FirstOrDefault(g =>
                g.UniekNummer == vm.UniekNummer &&
                g.Geboortedatum == vm.Geboortedatum);

            if (zwemmer == null)
            {
                ModelState.AddModelError(
                    "",
                    "Geen zwemmer gevonden met deze combinatie van geboortedatum en uniek nummer.");

                return View(vm);
            }

            bool isZwemmer = await _userManager.IsInRoleAsync(
                zwemmer,
                "Zwemmer");

            if (!isZwemmer)
            {
                ModelState.AddModelError(
                    "",
                    "Geen zwemmer gevonden met deze combinatie van geboortedatum en uniek nummer.");

                return View(vm);
            }

            List<VoogdZwemmer> gekoppeldeKinderen =
                await _unitOfWork.VoogdZwemmers.GetAllAsync();

            bool isAlGekoppeld = gekoppeldeKinderen.Any(k =>
                k.VoogdId == ouder.Id &&
                k.ZwemmerId == zwemmer.Id);

            if (isAlGekoppeld)
            {
                ModelState.AddModelError(
                    "",
                    "Dit kind is al gekoppeld aan je account.");

                return View(vm);
            }

            VoogdZwemmer gekoppeldKind = new VoogdZwemmer
            {
                VoogdId = ouder.Id,
                ZwemmerId = zwemmer.Id,
                IsMailGestuurd = false
            };

            _unitOfWork.VoogdZwemmers.Add(gekoppeldKind);

            await _unitOfWork.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"{zwemmer.Voornaam} is succesvol gekoppeld.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ToevoegenAsync()
        {
            await VulGeslachtenAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toevoegen(KindToevoegenViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await VulGeslachtenAsync();
                return View(vm);
            }

            Gebruiker? ouder = await _userManager.GetUserAsync(User);

            if (ouder == null)
            {
                return Unauthorized();
            }

            string uniekNummer = await GenereerUniekNummer();

            Gebruiker kind = vm.Adapt<Gebruiker>();

            kind.UniekNummer = uniekNummer;
            kind.UserName = uniekNummer;
            kind.IsActief = true;
            kind.IsVerwijderd = false;

            IdentityResult resultaat = await _userManager.CreateAsync(kind);

            if (!resultaat.Succeeded)
            {
                foreach (IdentityError fout in resultaat.Errors)
                {
                    ModelState.AddModelError(
                        "",
                        fout.Description);
                }

                return View(vm);
            }

            IdentityResult rolResultaat =
                await _userManager.AddToRoleAsync(
                    kind,
                    "Zwemmer");

            if (!rolResultaat.Succeeded)
            {
                foreach (IdentityError fout in rolResultaat.Errors)
                {
                    ModelState.AddModelError(
                        "",
                        fout.Description);
                }

                return View(vm);
            }

            VoogdZwemmer koppelingKind = new VoogdZwemmer
            {
                VoogdId = ouder.Id,
                ZwemmerId = kind.Id,
                IsMailGestuurd = false
            };

            _unitOfWork.VoogdZwemmers.Add(koppelingKind);

            await _unitOfWork.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"{kind.Voornaam} is succesvol toegevoegd. " +
                $"Het unieke nummer is {kind.UniekNummer}.";

            return RedirectToAction(nameof(Index));
        }
        private async Task<string> GenereerUniekNummer()
        {
            List<Gebruiker> gebruikers =
                await _unitOfWork.GebruikerRepository.GetAllAsync();

            int nummer = gebruikers.Count + 1;

            string uniekNummer = $"ZC-{nummer:D4}";

            while (gebruikers.Any(g =>
                g.UniekNummer == uniekNummer))
            {
                nummer++;
                uniekNummer = $"ZC-{nummer:D4}";
            }

            return uniekNummer;
        }

        [HttpGet]
        public async Task<IActionResult> Ontkoppelen(string id)
        {
            Gebruiker? ouder = await _userManager.GetUserAsync(User);

            if (ouder == null)
            {
                return Unauthorized();
            }

            Gebruiker? zwemmer =
                await _unitOfWork.GebruikerRepository.GetByIdAsync(id);

            if (zwemmer == null)
            {
                return NotFound();
            }

            List<VoogdZwemmer> gekoppeldeKinderen =
                await _unitOfWork.VoogdZwemmers.GetAllAsync();

            bool isGekoppeld = gekoppeldeKinderen.Any(k =>
                k.VoogdId == ouder.Id &&
                k.ZwemmerId == zwemmer.Id);

            if (!isGekoppeld)
            {
                return NotFound();
            }

            //if (string.IsNullOrWhiteSpace(zwemmer.Email) ||
            //    string.IsNullOrWhiteSpace(zwemmer.PhoneNumber))
            //{
            //    TempData["ErrorMessage"] =
            //        "Dit kind kan niet ontkoppeld worden. " +
            //        "Het e-mailadres en telefoonnummer moeten eerst ingevuld zijn.";

            //    return RedirectToAction(nameof(Index));
            //}

            KindViewModel vm = zwemmer.Adapt<KindViewModel>();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OntkoppelenBevestigd(string id)
        {
            Gebruiker? ouder = await _userManager.GetUserAsync(User);

            if (ouder == null)
            {
                return Unauthorized();
            }

            Gebruiker? zwemmer =
                await _unitOfWork.GebruikerRepository.GetByIdAsync(id);

            if (zwemmer == null)
            {
                return NotFound();
            }

            //if (string.IsNullOrWhiteSpace(zwemmer.Email) ||
            //    string.IsNullOrWhiteSpace(zwemmer.PhoneNumber))
            //{
            //    TempData["ErrorMessage"] =
            //        "Dit kind kan niet ontkoppeld worden. " +
            //        "Het e-mailadres en telefoonnummer moeten eerst ingevuld zijn.";

            //    return RedirectToAction(nameof(Index));
            //}

            List<VoogdZwemmer> gekoppeldeKinderen =
                await _unitOfWork.VoogdZwemmers.GetAllAsync();

            VoogdZwemmer? koppeling = gekoppeldeKinderen.FirstOrDefault(k =>
                k.VoogdId == ouder.Id &&
                k.ZwemmerId == zwemmer.Id);

            if (koppeling == null)
            {
                return NotFound();
            }

            _unitOfWork.VoogdZwemmers.Delete(koppeling);

            await _unitOfWork.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"{zwemmer.Voornaam} is succesvol ontkoppeld.";

            return RedirectToAction(nameof(Index));
        }

    }
}