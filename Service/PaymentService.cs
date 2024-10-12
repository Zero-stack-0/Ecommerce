using Microsoft.Extensions.Configuration;
using Stripe;

namespace Service
{
    public class PaymentService
    {
        private readonly string _stripeApiKey;
        public PaymentService(IConfiguration configuration)
        {
            _stripeApiKey = configuration["Stripe:SecretKey"];
        }

        public string CreatePaymentIntendId(decimal amount)
        {
            StripeConfiguration.ApiKey = _stripeApiKey;

            var options = new PaymentIntentCreateOptions
            {
                Amount = Convert.ToInt64(ConvertINRtoUSD(amount)),
                Currency = "USD",
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            return paymentIntent.Id;
        }

        private decimal ConvertINRtoUSD(decimal amountInINR, decimal conversionRate = 83.00m)
        {
            decimal amountInUSD = amountInINR / conversionRate;
            return Math.Round(amountInUSD, 2);
        }
    }
}