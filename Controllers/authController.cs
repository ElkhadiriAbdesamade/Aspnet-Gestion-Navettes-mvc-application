using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Gestion_Navettes.Models;

namespace Gestion_Navettes.Controllers
{
    
    public class authController : Controller
    {
        
        private Gestion_NavettesEntities db = new Gestion_NavettesEntities();
       

        public ActionResult Index()
        {
            return View();
        }
        // GET: auth
        
        [Route("Login")]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "abonnements");
            }

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [Route("Login")]
        [HttpPost]
        public ActionResult Login(users user,String returnUrl)
        {
            if (user.lgn==null)
            {
                ViewBag.lgn = "Email Not Valide !!";
                return View();
            }
            if (user.psw == null)
            {
                ViewBag.psw = "Psw Not Valide !!";
                return View();
            }
            var dataItem = db.users.Where(x => x.lgn == user.lgn && x.psw == user.psw).FirstOrDefault();
            if (dataItem != null)
            {
                FormsAuthentication.SetAuthCookie(dataItem.lgn, false);
                if (dataItem.roles=="User")
                {
                    System.Web.HttpContext.Current.Session["role"] = dataItem.roles.ToString();
                    return RedirectToAction("Index", "abonnements");
                }
                
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    System.Web.HttpContext.Current.Session["role"] = dataItem.roles.ToString();
                    return Redirect(returnUrl);
                }
                else
                {
                    System.Web.HttpContext.Current.Session["role"] = dataItem.roles.ToString();
                    return RedirectToAction("Index", "abonnements"); 
                }

            }
            else
            {
                ModelState.AddModelError("", "User Name Or Login Not Correct !!");
                return View();
            }
        }


        [Route("register")]
        public ActionResult register()
        {
            return View();
        }
        [Route("register")]
        [HttpPost]
        public ActionResult register(users user)
        {
            if (ModelState.IsValid)
            {


                if (db.users.Any(x => x.email == user.email))
                {
                    ViewBag.Notification = "Email Already Existed !!";
                    return View();
                }
                if (db.users.Any(x => x.lgn == user.lgn))
                {
                    ViewBag.Notification = "User Name Already Existed !!";
                    return View();
                }
                else
                {
                    user.roles = "User";
                    db.users.Add(user);
                    db.SaveChanges();
                    Session["Nom"] = user.nom_complet.ToString();
                    System.Web.HttpContext.Current.Session["role"] = user.roles.ToString();

                    return RedirectToAction("Index", "abonnements");
                }
            }
            return View(user);
        }

        [Route("SignOut")]
        [Authorize]
        public ActionResult SignOut()
        {
            System.Web.HttpContext.Current.Session["role"] = "";
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "abonnements");
        }
    }
}