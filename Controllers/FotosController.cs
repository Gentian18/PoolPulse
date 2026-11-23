using Zwemclub.ViewModels;

namespace Zwemclub.Controllers
{
    [AllowAnonymous]
    public class FotosController : Controller
    {
        // Voorbeeldbeelden: vervang deze later door clubfoto's met toestemming.
        private static readonly List<FotoViewModel> Fotos =
        [
            new FotoViewModel { Id = "zwembad", Categorie = "Zwembad", Titel = "Klaar voor de start", Bestand = "zwembad.png", Beschrijving = "Een leeg wedstrijdbad met startblokken" },
            new FotoViewModel { Id = "sfeer", Categorie = "Sfeer", Titel = "Aan de waterkant", Bestand = "sfeer.png", Beschrijving = "Zwembril en badmuts naast het water" },
            new FotoViewModel { Id = "podium", Categorie = "Podium", Titel = "Een moment om te vieren", Bestand = "podium.png", Beschrijving = "Medailles op een podium bij het zwembad" }
        ];

        public IActionResult Index(string? categorie)
        {
            var geselecteerdeCategorie = GeldigeCategorie(categorie);
            var fotos = geselecteerdeCategorie == "Alles"
                ? Fotos
                : Fotos.Where(foto => foto.Categorie == geselecteerdeCategorie).ToList();

            ViewData["Categorie"] = geselecteerdeCategorie;
            return View(fotos);
        }

        public IActionResult Details(string id, string? categorie)
        {
            var foto = Fotos.FirstOrDefault(foto => foto.Id == id);
            if (foto == null)
            {
                return NotFound();
            }

            ViewData["Categorie"] = GeldigeCategorie(categorie);
            return View(foto);
        }

        private static string GeldigeCategorie(string? categorie)
        {
            return Fotos.FirstOrDefault(foto => foto.Categorie.Equals(categorie, StringComparison.OrdinalIgnoreCase))?.Categorie ?? "Alles";
        }
    }
}
