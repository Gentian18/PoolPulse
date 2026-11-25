namespace Zwemclub.Services.Interface
{
    public interface IEmailService
    {
            Task VerstuurAsync(
            string ontvanger,
            string onderwerp,
            string bericht
        );
    }
}
