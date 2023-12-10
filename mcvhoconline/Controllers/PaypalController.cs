using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PayPal.Api;

public class PayPalController : Controller
{
    private readonly string clientId = "AfdT35N7U4ONak13bE5ZiSXtzIzLlwNz93_k2aKCEMYBWicE4eexgXFnHvjLNE3itmwpcXxkEYMJzTEQ";
    private readonly string clientSecret = "EBDALI8S4FTsQMpIJNKW7LwlqLkI94mVGTKLzUX75_yMeueRUktO0S5fIFQMx03d6TlTuSXjYJXvmYNV";
    
    public ActionResult Index()
    {
        return View();
    }

    public ActionResult CreatePayment()
    {
        var apiContext = GetAPIContext();

        var payment = Payment.Create(apiContext, new Payment
        {
            intent = "sale",
            payer = new Payer { payment_method = "paypal" },
            transactions = new List<Transaction>
            {
                new Transaction
                {
                    description = "Transaction description",
                    amount = new Amount
                    {
                        currency = "USD",
                        total = "10.00" // Replace with your amount
                    }
                }
            },
            redirect_urls = new RedirectUrls
            {
                return_url = Url.Action("PaymentSuccess", "PayPal", null, Request.Url.Scheme),
                cancel_url = Url.Action("PaymentCancelled", "PayPal", null, Request.Url.Scheme)
            }
        });

        return Redirect(payment.GetApprovalUrl());
    }

    public ActionResult PaymentSuccess(string paymentId, string token, string PayerID)
    {
        var apiContext = GetAPIContext();

        var paymentExecution = new PaymentExecution { payer_id = PayerID };
        var payment = new Payment { id = paymentId };

        var executedPayment = payment.Execute(apiContext, paymentExecution);

        // Perform actions after successful payment
        return RedirectToAction("Index");
    }

    public ActionResult PaymentCancelled()
    {
        // Handle payment cancellation
        return RedirectToAction("Index");
    }

    private APIContext GetAPIContext()
    {
        var config = new Dictionary<string, string>
        {
            {"mode", "sandbox"}, // Change to "live" for production
            {"clientId", clientId},
            {"clientSecret", clientSecret}
        };

        var accessToken = new OAuthTokenCredential(config).GetAccessToken();
        return new APIContext(accessToken) { Config = config };
    }
}
