
using Zwemclub.ViewModels;

namespace Zwemclub.Controllers
{
    [Authorize(Roles = "Voorzitter")]
    public class WedstrijdBeheerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WedstrijdBeheerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string? zoekterm)
        {
            var wedstrijden = await _unitOfWork.Wedstrijden.GetAllAsync();
            var plaatsen = await _unitOfWork.Plaatsen.GetAllAsync();

            var plaatsenPerId = plaatsen.ToDictionary(plaats => plaats.Id);

            var overzicht = wedstrijden
                .Where(wedstrijd => !wedstrijd.IsVerwijderd)
                .Where(wedstrijd =>
                    string.IsNullOrWhiteSpace(zoekterm)
                    || wedstrijd.Naam.Contains(
                        zoekterm.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                .OrderBy(wedstrijd => wedstrijd.Datum)
                .Select(wedstrijd => new WedstrijdOverzichtViewModel
                {
                    Id = wedstrijd.Id,
                    Naam = wedstrijd.Naam,
                    Datum = wedstrijd.Datum,
                    Locatie = plaatsenPerId.TryGetValue(
                        wedstrijd.PlaatsId,
                        out var plaats)
                        ? plaats.Gemeente
                        : "Locatie niet gevonden"
                })
                .ToList();

            ViewData["Zoekterm"] = zoekterm;

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
    }
}