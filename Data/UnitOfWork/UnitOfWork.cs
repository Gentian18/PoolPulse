using Zwemclub.Data.Repository.Interfaces;

namespace Zwemclub.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ZwemclubContext _context;

        public UnitOfWork(ZwemclubContext context)
        {
            _context = context;
        }

        
        private IGebruikerRepository? _gebruikerRepository;
        private IGenericRepository<GebruikerCarpool>? gebruikerCarpools;
        private IGenericRepository<Parameter>? parameters;
        private IGenericRepository<TrainerGroep>? trainerGroepen;
        private IGenericRepository<ReeksType>? reeksTypes;
        private IGenericRepository<InschrijvingWedstrijd>? inschrijvingWedstrijden;
        private IGenericRepository<WedstrijdReeks>? wedstrijdReeksen;
        private IGenericRepository<Carpool>? carpools;
        private IGenericRepository<Mail>? mails;
        private IGenericRepository<VoogdZwemmer>? voogdZwemmers;
        private IGenericRepository<Plaats>? plaatsen;
        private IGenericRepository<Geslacht>? geslachten;
        private IGenericRepository<Wedstrijd>? wedstrijden;


        public IGebruikerRepository GebruikerRepository => _gebruikerRepository
            ??= new GebruikerRepository(_context);

        public IGenericRepository<GebruikerCarpool> GebruikerCarpools =>
            gebruikerCarpools ??= new GenericRepository<GebruikerCarpool>(_context);

        public IGenericRepository<Parameter> Parameters =>
            parameters ??= new GenericRepository<Parameter>(_context);

        public IGenericRepository<TrainerGroep> TrainerGroepen =>
            trainerGroepen ??= new GenericRepository<TrainerGroep>(_context);

        public IGenericRepository<ReeksType> ReeksTypes =>
            reeksTypes ??= new GenericRepository<ReeksType>(_context);

        public IGenericRepository<InschrijvingWedstrijd> InschrijvingWedstrijden =>
            inschrijvingWedstrijden ??= new GenericRepository<InschrijvingWedstrijd>(_context);

        public IGenericRepository<WedstrijdReeks> WedstrijdReeksen =>
            wedstrijdReeksen ??= new GenericRepository<WedstrijdReeks>(_context);

        public IGenericRepository<Carpool> Carpools =>
            carpools ??= new GenericRepository<Carpool>(_context);

        public IGenericRepository<Mail> Mails =>
            mails ??= new GenericRepository<Mail>(_context);

        public IGenericRepository<VoogdZwemmer> VoogdZwemmers =>
            voogdZwemmers ??= new GenericRepository<VoogdZwemmer>(_context);

        public IGenericRepository<Plaats> Plaatsen =>
            plaatsen ??= new GenericRepository<Plaats>(_context);

        public IGenericRepository<Geslacht> Geslachten =>
            geslachten ??= new GenericRepository<Geslacht>(_context);

        public IGenericRepository<Wedstrijd> Wedstrijden =>
            wedstrijden ??= new GenericRepository<Wedstrijd>(_context);


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
