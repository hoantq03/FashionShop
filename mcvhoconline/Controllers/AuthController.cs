﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mcvhoconline.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        public ActionResult ConfirmPassword()
        {
            return View();
        }
    }
}