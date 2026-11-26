using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zwemclub.ViewModels.Kinderen
{
    public class KinderenViewModel
    {
        public List<KindViewModel> Kinderen { get; set; } = [];

        public KindToevoegenViewModel NieuwKind { get; set; } = new();

        public List<SelectListItem> Geslachten { get; set; } = [];

        public bool ToonToevoegenModal { get; set; }
    }
}