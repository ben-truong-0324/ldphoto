using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System.Collections.Generic;

namespace Portfolio.Core.Services
{
    public class PayPalService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly bool _isSandbox;

        public PayPalService(IConfiguration configuration)
        {
            _clientId = configuration["PayPal:ClientId"];
            _clientSecret = configuration["PayPal:ClientSecret"];
            _isSandbox = configuration.GetValue<bool>("PayPal:IsSandbox");
        }

        private PayPalEnvironment GetEnvironment()
        {
            return _isSandbox ? new SandboxEnvironment(_clientId, _clientSecret) 
                            : new LiveEnvironment(_clientId, _clientSecret);
        }

        public async Task<Order> CreateOrder(decimal amount, string currency = "USD")
        {
            var environment = GetEnvironment();
            var client = new PayPalHttpClient(environment);

            var orderRequest = new OrdersCreateRequest();
            orderRequest.Prefer("return=representation");
            orderRequest.RequestBody(new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = currency,
                            Value = amount.ToString("F2")
                        }
                    }
                }
            });

            var response = await client.Execute(orderRequest);
            return response.Result<Order>();
        }

        public async Task<Order> CaptureOrder(string orderId)
        {
            var environment = GetEnvironment();
            var client = new PayPalHttpClient(environment);

            var request = new OrdersCaptureRequest(orderId);
            request.RequestBody(new OrderActionRequest());

            var response = await client.Execute(request);
            return response.Result<Order>();
        }
    }
}