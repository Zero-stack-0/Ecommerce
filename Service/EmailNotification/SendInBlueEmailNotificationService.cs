using Microsoft.Extensions.Configuration;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace Service;

public class SendInBlueEmailNotificationService
{
    private readonly TransactionalEmailsApi _apiInstance;
    private readonly string _senderName;
    private readonly string _senderEmail;

    public SendInBlueEmailNotificationService(IConfiguration configuration)
    {
        var apiKey = configuration["Brevo:ApiKey"];
        Configuration.Default.ApiKey.Add("api-key", apiKey);

        _apiInstance = new TransactionalEmailsApi();
        _senderName = configuration["Brevo:SenderName"];
        _senderEmail = configuration["Brevo:SenderEmail"];
    }

    public void SendEmail(string toEmail, string toName, string subject, string htmlContent)
    {
        var sender = new SendSmtpEmailSender(_senderName, _senderEmail);
        var smtpEmailTo = new SendSmtpEmailTo(toEmail, toName);
        var to = new List<SendSmtpEmailTo> { smtpEmailTo };

        var sendSmtpEmail = new SendSmtpEmail(
                sender,
                to,
                null,
                null,
                htmlContent,
                null,
                subject
            );

        _apiInstance.SendTransacEmail(sendSmtpEmail);

    }
}

