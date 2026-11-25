using Zwemclub.ViewModels;

namespace Zwemclub.Controllers
{
    //[Authorize(Roles = "Voorzitter")]
    public class WedstrijdBeheerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly UserManager<CustomUser> _userManager;
        private readonly ILogger<WedstrijdBeheerController> _logger;

        public WedstrijdBeheerController(
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            UserManager<CustomUser> userManager,
            ILogger<WedstrijdBeheerController> logger)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
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

            var mailsGelukt = await MailTrainersAsync(wedstrijd, plaats);

            if (mailsGelukt)
            {
                TempData["Melding"] = "De wedstrijd is opgeslagen.";
            }
            else
            {
                TempData["Melding"] =
                    "De wedstrijd is opgeslagen, maar een of meerdere e-mails zijn mislukt.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MailTrainersAsync(
            Wedstrijd wedstrijd,
            Plaats plaats)
        {
            var allesGelukt = true;

            try
            {
                var trainers = await _userManager.GetUsersInRoleAsync("Trainer");

                foreach (var trainer in trainers)
                {
                    if (!trainer.IsActief ||
                        trainer.IsVerwijderd == true ||
                        string.IsNullOrWhiteSpace(trainer.Email))
                    {
                        continue;
                    }

                    var onderwerp = "Nieuwe wedstrijd: " + wedstrijd.Naam;

                    var bericht =
                        "Hallo " + trainer.Voornaam + "," +
                        Environment.NewLine + Environment.NewLine +
                        "Er is een nieuwe wedstrijd toegevoegd." +
                        Environment.NewLine +
                        "Wedstrijd: " + wedstrijd.Naam +
                        Environment.NewLine +
                        "Datum: " + wedstrijd.Datum.ToString("dd/MM/yyyy") +
                        Environment.NewLine +
                        "Locatie: " + plaats.Naam + ", " + plaats.Gemeente +
                        Environment.NewLine +
                        "Deadline inschrijving: " +
                        wedstrijd.DeadlineInschrijven.ToString("dd/MM/yyyy HH:mm");

                    try
                    {
                        await _emailService.VerstuurAsync(
                            trainer.Email,
                            onderwerp,
                            bericht
                        );
                    }
                    catch (Exception exception)
                    {
                        allesGelukt = false;

                        _logger.LogError(
                            exception,
                            "Een e-mail voor wedstrijd {WedstrijdId} is mislukt.",
                            wedstrijd.Id
                        );
                    }
                }
            }
            catch (Exception exception)
            {
                allesGelukt = false;

                _logger.LogError(
                    exception,
                    "De trainers voor wedstrijd {WedstrijdId} konden niet worden opgehaald.",
                    wedstrijd.Id
                );
            }

            return allesGelukt;
        }
    }
}