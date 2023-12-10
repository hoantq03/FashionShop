using mcvhoconline.Models;
using System.Linq;
using System.Web.Mvc;


public class SessionAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationContext filterContext)
    {
        string Email = filterContext.HttpContext.Session["user"] as string;

        if (string.IsNullOrEmpty(Email))
        {
            // Redirect to login page or handle unauthorized access
            filterContext.Result = new RedirectResult("/account/login");
        }
        else
        {
            // You can perform additional checks or load user information here
            ShopEntities5 _shopContext = new ShopEntities5();
            user khachHang = _shopContext.users.FirstOrDefault(user => user.email == Email);

            if (khachHang == null)
            {
                // Redirect to login page or handle unauthorized access
                filterContext.Result = new RedirectResult("/account/login");
            }
            else
            {
                // You can store the user in the ViewBag or ViewData for use in your actions
                filterContext.HttpContext.Items["CurrentUser"] = khachHang;
                filterContext.Controller.ViewBag.CurrentUser = khachHang;
            }
        }
    }
}