using PoliziaMunicipale_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliziaMunicipale_MVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login(Login login)
        {
            string correctUsername = ConfigurationManager.AppSettings["username"];
            string correctPassword = ConfigurationManager.AppSettings["password"];

            if(login.Username == correctUsername || login.Password == correctPassword)
            {
                Session["isAuthenticated"] = true;
                string controllerDorigine = TempData["controllerDorigine"] as string;
                System.Diagnostics.Debug.WriteLine("ControllerDorigine: " + controllerDorigine);
                if (!string.IsNullOrEmpty(controllerDorigine))
                {
                    return RedirectToAction("Create", controllerDorigine);
                }
                else { 
                return RedirectToAction("Login", "Login"); }
            }
            else
            {
                ViewBag.MessaggioErrore = "Username o password non validi";
            }

            return View();
        }
    }
}