using Zwemclub.Data.Repository.Interfaces;
using Zwemclub.ViewModels;

namespace Zwemclub.Data.Repository
{
    public class WedstrijdRepository(ZwemclubContext context) : GenericRepository<Wedstrijd>(context), IWedstrijdRepository
    {
        public Task<List<WedstrijdOverzichtViewModel>> GetPubliekOverzichtAsync()
        {
            return _context.Wedstrijden.AsNoTracking()
                .Where(wedstrijd => !wedstrijd.IsVerwijderd)
                .OrderBy(wedstrijd => wedstrijd.Datum)
                .ThenBy(wedstrijd => wedstrijd.Naam)
                .Select(wedstrijd => new WedstrijdOverzichtViewModel
                {
                    Id = wedstrijd.Id,
                    Naam = wedstrijd.Naam,
                    Datum = wedstrijd.Datum,
                    Locatie = wedstrijd.Plaats.Gemeente
                }).ToListAsync();
        }

        public Task<WedstrijdDetailsViewModel?> GetPubliekeDetailsAsync(int id)
        {
            return _context.Wedstrijden.AsNoTracking()
                .Where(wedstrijd => wedstrijd.Id == id && !wedstrijd.IsVerwijderd)
                .Select(wedstrijd => new WedstrijdDetailsViewModel
                {
                    Naam = wedstrijd.Naam,
                    Datum = wedstrijd.Datum,
                    Locatie = wedstrijd.Plaats.Naam,
                    Adres = wedstrijd.Plaats.Straat + " " + wedstrijd.Plaats.HuisNummer + ", " + wedstrijd.Plaats.Postcode + " " + wedstrijd.Plaats.Gemeente,
                    Reeksen = wedstrijd.Reeksen.Where(reeks => !reeks.IsVerwijderd)
                        .OrderBy(reeks => reeks.Startuur).ThenBy(reeks => reeks.Id)
                        .Select(reeks => new ReeksViewModel
                        {
                            Naam = reeks.ReeksType.Naam,
                            Geslacht = reeks.Geslacht.Naam,
                            Startuur = reeks.Startuur,
                            Deelnemers = reeks.Inschrijvingen
                                .Where(inschrijving => inschrijving.DatumDefinitief != null && inschrijving.DatumAnnulatie == null && !inschrijving.IsVerwijderd && inschrijving.Zwemmer.IsVerwijderd != true)
                                .OrderBy(inschrijving => inschrijving.Zwemmer.Naam)
                                .ThenBy(inschrijving => inschrijving.Zwemmer.Voornaam)
                                .Select(inschrijving => new DeelnemerViewModel
                                {
                                    Voornaam = inschrijving.Zwemmer.Voornaam,
                                    Naam = inschrijving.Zwemmer.Naam,
                                    BehaaldeTijd = inschrijving.BehaaldeTijd,
                                    BehaaldePlaats = inschrijving.BehaaldePlaats
                                }).ToList()
                        }).ToList()
                }).SingleOrDefaultAsync();
        }
    }
}
