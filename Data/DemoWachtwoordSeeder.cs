namespace Zwemclub.Data
{
    // Alleen voor ontwikkeling: de voorbeeldprofielen in de seed hebben geen wachtwoord.
    // Stel DemoWachtwoord in via user-secrets; zonder die waarde gebeurt er niets.
    public static class DemoWachtwoordSeeder
    {
        private static readonly string[] DemoEmails =
        [
            "ouder@zwemclub.example",
            "zwemmer@zwemclub.example",
            "trainer@zwemclub.example",
            "voorzitter@zwemclub.example"
        ];

        public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
        {
            var wachtwoord = configuration["DemoWachtwoord"];
            if (string.IsNullOrWhiteSpace(wachtwoord))
            {
                return;
            }

            using var scope = services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CustomUser>>();

            foreach (var email in DemoEmails)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user is null || await userManager.HasPasswordAsync(user))
                {
                    continue;
                }

                await userManager.AddPasswordAsync(user, wachtwoord);
            }
        }
    }
}