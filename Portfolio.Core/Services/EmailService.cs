using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Portfolio.Core.Models;

namespace Portfolio.Core.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Portfolio Shop", _configuration["Email:From"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_configuration["Email:SmtpServer"], 
                                      int.Parse(_configuration["Email:SmtpPort"] ?? "587"), 
                                      SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(_configuration["Email:Username"], _configuration["Email:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendOrderConfirmationAsync(string toEmail, Order order, bool isAdminCopy = false)
        {
            string title = isAdminCopy ? "New Sale Alert" : "Order Confirmed";
            string subject = isAdminCopy ? $"[SALE] {order.OrderNumber} - {order.TotalAmount:C}" : $"Order Confirmation - {order.OrderNumber}";
            
            string htmlContent = BuildProfessionalEmailHtml(order, title);
            await SendEmailAsync(toEmail, subject, htmlContent);
        }

        // private string GetFullImageUrl(string? imageUrl)
        // {
        //     const string FallbackUrl = "https://www.shutterstock.com/image-photo/sad-cute-brown-dog-look-260nw-2478688715.jpg";

        //     if (string.IsNullOrWhiteSpace(imageUrl)) return FallbackUrl;
        //     if (imageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return imageUrl;

        //     string baseUrl = _configuration["SiteUrl"]?.TrimEnd('/') ?? "https://localhost:44359";
        //     return $"{baseUrl}/{imageUrl.TrimStart('/')}";
        // }
        private string GetFullImageUrl(string? imageUrl)
        {
            const string FallbackUrl = "https://www.shutterstock.com/image-photo/sad-cute-brown-dog-look-260nw-2478688715.jpg";

            if (string.IsNullOrWhiteSpace(imageUrl)) return FallbackUrl;
            
            // FIX 1: URL Encode the spaces so HTML email clients don't break the tag
            string cleanPath = imageUrl.Replace(" ", "%20");

            if (cleanPath.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return cleanPath;

            string baseUrl = _configuration["SiteUrl"]?.TrimEnd('/') ?? "https://localhost:44359";
            return $"{baseUrl}/{cleanPath.TrimStart('/')}";
        }

        private string BuildProfessionalEmailHtml(Order order, string title)
        {
            var itemsHtml = string.Join("", order.OrderItems.Select(item => {
                string absoluteImgUrl = GetFullImageUrl(item.ImageUrl);

                // Build the options string dynamically
                string optionsHtml = $"Format: {item.Format}";
                if (!string.IsNullOrEmpty(item.Size)) optionsHtml += $" ({item.Size})";
                optionsHtml += "<br/>";
                if (item.Finish != PrintFinish.None) optionsHtml += $"Finish: {item.Finish}<br/>";
                if (item.IsFramed) optionsHtml += $"<strong style='color: #28a745;'>Framed</strong><br/>";

                return $@"
                <div style='display: block; margin-bottom: 20px; border-bottom: 1px solid #eee; padding-bottom: 20px;'>
                    <table width='100%' cellspacing='0' cellpadding='0' border='0'>
                        <tr>
                            <td width='100' style='vertical-align: top;'>
                                <img src='{absoluteImgUrl}' 
                                    alt='{item.ProductName}' 
                                    width='80' 
                                    style='border-radius: 8px; display: block; background-color: #f0f0f0;' />
                            </td>
                            <td style='padding-left: 20px; vertical-align: top;'>
                                <h4 style='margin: 0 0 5px 0; font-size: 16px; color: #333;'>{item.ProductName}</h4>
                                <p style='margin: 0 0 5px 0; font-size: 13px; color: #666;'>{optionsHtml}</p>
                                <p style='margin: 0; font-size: 14px; color: #666;'>Quantity: {item.Quantity}</p>
                                <p style='margin: 5px 0 0 0; font-size: 14px; font-weight: bold; color: #111;'>{(item.Price * item.Quantity):C}</p>
                            </td>
                        </tr>
                    </table>
                </div>";
            }));

            return $@"
            <div style='background-color: #f9f9f9; padding: 40px 0; font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, Helvetica, Arial, sans-serif;'>
                <table align='center' width='600' cellspacing='0' cellpadding='0' border='0' style='background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 15px rgba(0,0,0,0.05);'>
                    <tr>
                        <td style='padding: 40px; text-align: center; border-bottom: 1px solid #f0f0f0;'>
                            <h1 style='margin: 0; font-size: 24px; color: #111; letter-spacing: -0.5px;'>{title}</h1>
                            <p style='color: #666; margin-top: 10px;'>Order #{order.OrderNumber}</p>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding: 40px;'>
                            <p style='font-size: 16px; color: #444; line-height: 1.6;'>
                                Hi {order.CustomerName},<br />
                                Your order has been received and is currently being processed. You’ll receive another update once your prints are ready for shipping.
                            </p>
                            
                            <div style='margin-top: 40px;'>
                                <h3 style='font-size: 14px; text-transform: uppercase; letter-spacing: 1px; color: #999; margin-bottom: 20px;'>Your Selection</h3>
                                {itemsHtml}
                            </div>

                            <table width='100%' cellspacing='0' cellpadding='0' border='0' style='margin-top: 20px;'>
                                <tr>
                                    <td style='font-size: 16px; color: #666;'>Total Amount Paid</td>
                                    <td align='right' style='font-size: 20px; font-weight: bold; color: #007aff;'>{order.TotalAmount:C}</td>
                                </tr>
                            </table>

                            <hr style='border: 0; border-top: 1px solid #eee; margin: 40px 0;' />

                            <table width='100%' cellspacing='0' cellpadding='0' border='0'>
                                <tr>
                                    <td width='50%'>
                                        <h4 style='margin: 0; font-size: 12px; text-transform: uppercase; color: #999;'>Customer</h4>
                                        <p style='margin: 5px 0 0 0; font-size: 14px; color: #333;'>{order.CustomerName}</p>
                                        <p style='margin: 2px 0 0 0; font-size: 14px; color: #333;'>{order.CustomerEmail}</p>
                                    </td>
                                    <td width='50%' style='text-align: right;'>
                                        <h4 style='margin: 0; font-size: 12px; text-transform: uppercase; color: #999;'>Date</h4>
                                        <p style='margin: 5px 0 0 0; font-size: 14px; color: #333;'>{order.OrderDate:MMM dd, yyyy}</p>
                                    </td>
                                </tr>
                            </table>
                            <table width='100%' cellspacing='0' cellpadding='0' border='0' style='margin-top: 25px;'>
                                <tr>
                                    <td width='100%' style='background-color: #f4f6f8; padding: 20px; border-radius: 8px;'>
                                        <h4 style='margin: 0; font-size: 12px; text-transform: uppercase; color: #999;'>Shipping To</h4>
                                        <p style='margin: 5px 0 0 0; font-size: 14px; color: #333; line-height: 1.5;'>
                                            <strong>{order.CustomerName}</strong><br/>
                                            {order.ShippingAddress}<br/>
                                            {order.ShippingCity}, {order.ShippingState} {order.ShippingZip}
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding: 30px; background-color: #fafafa; text-align: center;'>
                            <p style='font-size: 12px; color: #aaa; margin: 0;'>
                                &copy; {DateTime.UtcNow.Year} LDPhotography. All rights reserved. <br />
                                This is a secure transaction via PayPal.
                            </p>
                        </td>
                    </tr>
                </table>
            </div>";
        }
    }
}