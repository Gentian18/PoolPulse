
using Zwemclub.ViewModels;
using Zwemclub.Models;

namespace Zwemclub.Controllers
{
    //[Authorize(Roles = "Voorzitter")]
    public class WedstrijdBeheerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WedstrijdBeheerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string? zoekterm, int? jaar)
        {
            var wedstrijden = await _unitOfWork.Wedstrijden.GetAllAsync();
            var plaatsen = await _unitOfWork.Plaatsen.GetAllAsync();

            var overzicht = new List<WedstrijdOverzichtViewModel>();

            foreach (var wedstrijd in wedstrijden)
            {
                if (wedstrijd.IsVerwijderd)
                {
                    continue;
                }

                if (jaar.HasValue && wedstrijd.Datum.Year != jaar.Value)
                {
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(zoekterm) &&
                    !wedstrijd.Naam.Contains(
                        zoekterm.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var plaats = plaatsen.Find(p => p.Id == wedstrijd.PlaatsId);
                var locatie = "Locatie niet gevonden";

                if (plaats != null)
                {
                    locatie = plaats.Gemeente;
                }

                var wedstrijdVoorOverzicht = new WedstrijdOverzichtViewModel
                {
                    Id = wedstrijd.Id,
                    Naam = wedstrijd.Naam,
                    Datum = wedstrijd.Datum,
                    Locatie = locatie
                };

                overzicht.Add(wedstrijdVoorOverzicht);
            }

            overzicht = overzicht.OrderBy(wedstrijd => wedstrijd.Datum).ToList();

            ViewData["Zoekterm"] = zoekterm;
            ViewData["Jaar"] = jaar;

            return View(overzicht);
        }

        public async Task<IActionResult> Details(int id)
        {
            var wedstrijd = await _unitOfWork.Wedstrijden.GetByIdAsync(id);

            if (wedstrijd == null)
            {
                return NotFound();
            }

            if (wedstrijd.IsVerwijderd)
            {
                return NotFound();
            }

            var plaats = await _unitOfWork.Plaatsen.GetByIdAsync(
                wedstrijd.PlaatsId
            );

            ViewData["Plaats"] = plaats;

            return View(wedstrijd);
        }

        [HttpGet]
        public IActionResult Toevoegen()
        {
            var formulier = new WedstrijdToevoegenViewModel();

            return View(formulier);
        }

        [HttpPost]
        public async Task<IActionResult> Toevoegen(
            WedstrijdToevoegenViewModel formulier)
        {
            if (!ModelState.IsValid)
            {
                return View(formulier);
            }

            if (formulier.Datum == null || formulier.DeadlineInschrijven == null)
            {
                return View(formulier);
            }

            var plaats = new Plaats
            {
                Naam = formulier.PlaatsNaam,
                Straat = formulier.Straat,
                HuisNummer = formulier.HuisNummer,
                Postcode = formulier.Postcode,
                Gemeente = formulier.Gemeente,
                Land = formulier.Land
            };

            var wedstrijd = new Wedstrijd
            {
                Naam = formulier.Naam,
                Datum = formulier.Datum.Value,
                DeadlineInschrijven = formulier.DeadlineInschrijven.Value,
                UrlWedstrijd = formulier.UrlWedstrijd,
                Plaats = plaats
            };

            _unitOfWork.Wedstrijden.Add(wedstrijd);

            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}