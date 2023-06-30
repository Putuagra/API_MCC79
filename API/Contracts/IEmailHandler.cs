namespace API.Contracts;

public interface IEmailHandler
{
    public void SendEmail(string email, string subject, string htmlMessage);
}
