using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using mcvhoconline.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

public class GoogleOAuthController : Controller
{
    private static readonly string[] Scopes = { Oauth2Service.Scope.UserinfoProfile, Oauth2Service.Scope.UserinfoEmail };
    private ShopEntities5 _fashionShop;
    public GoogleOAuthController()
    {
        _fashionShop = new ShopEntities5();
    }

    [HttpGet]
    public ActionResult SignIn()    
    {
        var authCallbackUri = new Uri(Url.Action(nameof(HandleGoogleCallbackAsync), "GoogleOAuth", null, Request.Url.Scheme));
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets { ClientId = "296417493435-t47tqrng3nsqb4cbu8khee02cb1582fn.apps.googleusercontent.com", ClientSecret = "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" },
            Scopes = Scopes,
        });

        var authUri = flow.CreateAuthorizationCodeRequest(authCallbackUri.ToString()).Build();
        return new RedirectResult(authUri.ToString());
    }

    [HttpGet]
    public async Task<ActionResult> HandleGoogleCallbackAsync(string code, CancellationToken cancellationToken)
    {
        var authCallbackUri = Url.Action(nameof(HandleGoogleCallbackAsync), "GoogleOAuth", null, Request.Url.Scheme);
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets { ClientId = "296417493435-t47tqrng3nsqb4cbu8khee02cb1582fn.apps.googleusercontent.com", ClientSecret = "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" },
            Scopes = Scopes,
        });

        var token = await flow.ExchangeCodeForTokenAsync(User.Identity.Name, code, authCallbackUri, cancellationToken);

        // Now you can use the token to make authenticated requests to Google APIs
        var userInfo = await new Oauth2Service(new Google.Apis.Services.BaseClientService.Initializer
        {
            HttpClientInitializer = GoogleCredential.FromAccessToken(token.AccessToken)
        }).Userinfo.Get().ExecuteAsync();
         
        // Example: Retrieve user information
        var userId = userInfo.Id;
        var userEmail = userInfo.Email;
        var userName = userInfo.Name;


        // check user is exist
        user user = _fashionShop.users.ToList().Find(u => u.email == userEmail);
        if (user == null)
        {
            cart newCart = new cart();
            newCart.id = Guid.NewGuid().ToString();
            newCart.total_amount = 0;
            newCart.created_at = DateTime.Now;
            newCart.created_by = "1";
            newCart.updated_by = "1";
            newCart.updated_at = DateTime.Now;

            _fashionShop.carts.Add(newCart);

            string[] nameSplitted = userName.Split(' ');
            user newUser = new user();
            newUser.id = Guid.NewGuid().ToString();
            newUser.email = userEmail;
            newUser.created_at = DateTime.UtcNow;
            newUser.first_name = userName.Split(' ')[0];
            newUser.last_name = string.Join(" ", nameSplitted,1,nameSplitted.Length - 1);
            newUser.status = 1;
            newUser.cart_id = newCart.id;
            newUser.password = Guid.NewGuid().ToString();
            newUser.phone = "";
            newUser.role = "customer";
            newUser.created_at = DateTime.Now;
            newUser.created_by = "1";
            newUser.updated_by = "1";
            newUser.updated_at = DateTime.Now;

            _fashionShop.users.Add(newUser);
            _fashionShop.SaveChanges();

            Session["currentUser"] = newUser;
        }


        return Redirect("/");
    }


    [HttpGet]
    public ActionResult SignOut()
    {
        Session["currentUser"] = null;

        return Redirect("/");
    }
}
