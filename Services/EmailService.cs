using System.Net;
using System.Net.Mail;
using Zwemclub.Services.Interface;

namespace Zwemclub.Services

{
    public class EmailService:IEmailService
    {
    private readonly IConfiguration _configuratie;

    public EmailService(IConfiguration configuratie)
    {
        _configuratie = configuratie;
    }

    public async Task VerstuurAsync(
        string ontvanger,
        string onderwerp,
        string bericht)
    {
        var server = _configuratie["Email:Server"];
        var afzender = _configuratie["Email:Afzender"];

        if (string.IsNullOrWhiteSpace(server))
        {
            throw new InvalidOperationException(
                "De e-mailserver is niet ingesteld."
            );
        }

        if (string.IsNullOrWhiteSpace(afzender))
        {
            throw new InvalidOperationException(
                "Het e-mailadres van de afzender is niet ingesteld."
            );
        }

        var poortTekst = _configuratie["Email:Poort"];

        if (!int.TryParse(poortTekst, out var poort))
        {
            throw new InvalidOperationException(
                "De poort van de e-mailserver is niet geldig."
            );
        }

        var gebruikersnaam = _configuratie["Email:Gebruikersnaam"];
        var wachtwoord = _configuratie["Email:Wachtwoord"];

        var gebruikSsl = _configuratie.GetValue<bool>("Email:GebruikSsl");

        using var mail = new MailMessage();

        mail.From = new MailAddress(afzender);
        mail.To.Add(ontvanger);
        mail.Subject = onderwerp;
        mail.Body = bericht;

        using var smtpClient = new SmtpClient(server, poort);

        smtpClient.EnableSsl = gebruikSsl;

        if (!string.IsNullOrWhiteSpace(gebruikersnaam))
        {
            smtpClient.Credentials = new NetworkCredential(
                gebruikersnaam,
                wachtwoord
            );
        }

        await smtpClient.SendMailAsync(mail);
    }
}
}
