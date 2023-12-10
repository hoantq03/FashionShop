using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mcvhoconline.Models
{
    public class SessionAuthorizeAdminAttribute : ActionFilterAttribute, IAuthorizationFilter
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
                user curUser = _shopContext.users.FirstOrDefault(user => user.email == Email);

                if (curUser == null)
                {
                    // Redirect to login page or handle unauthorized access
                    filterContext.Result = new RedirectResult("/");
                }
                if(curUser.role != "Admin")
                {
                    // You can store the user in the ViewBag or ViewData for use in your actions
                    filterContext.HttpContext.Items["CurrentUser"] = curUser;
                    filterContext.Controller.ViewBag.CurrentUser = curUser;
                }
                else
                {
                    filterContext.Result = new RedirectResult("/");
                }
            }
        }
    }
}