using PayPal.Api;
using System.Collections.Generic;
using System;

public class PayPalConfig
{
    public static APIContext GetAPIContext()
    {
        var clientId = "YOUR_CLIENT_ID";
        var clientSecret = "YOUR_CLIENT_SECRET";

        var config = new Dictionary<string, string>
        {
            {"mode", "sandbox"}, // Change to "live" for production
            {"clientId", clientId},
            {"clientSecret", clientSecret},
        };

        var accessToken = new OAuthTokenCredential(config).GetAccessToken();
        var apiContext = new APIContext(accessToken)
        {
            Config = config
        };

        return apiContext;
    }
}
