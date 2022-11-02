using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gestion_Navettes.Models;

namespace Gestion_Navettes.Controllers
{
    
    [RoutePrefix("reservations")]
    public class reservationsController : Controller
    {
        private Gestion_NavettesEntities db = new Gestion_NavettesEntities();

        // GET: reservations
        [Route()]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            if (ViewBag.Notification == null)
            {
                ViewBag.Notification = "";
            }
            var reservation = db.reservation.Include(r => r.abonnement).Include(r => r.users);
            return View(reservation.ToList());
        }
        [Route("MyReserve")]
        public ActionResult Index(users user)
        {
            if (ViewBag.Notification == null)
            {
                ViewBag.Notification = "";
            }
            String lgn = HttpContext.User.Identity.Name;
            var dataItem_u = db.users.Where(x => x.lgn == lgn).First();
            var reservation = db.reservation.Include(r => r.abonnement).Include(r => r.users).Where(x=>x.id_user==dataItem_u.id_user);
            return View(reservation.ToList());
        }

        // GET: reservations/Details/5
        [Route("details/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reservation reservation = db.reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: reservations/Create
        [Route("ajouter_a")]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            if (ViewBag.Notification==null)
            {
                ViewBag.Notification = "";
            }
            
            ViewBag.id_abn = new SelectList(db.abonnement, "id_abn", "ville_depart");
            ViewBag.id_user = new SelectList(db.users, "id_user", "nom_complet");
            return View();
        }
        

        // POST: reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajouter_a")]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(reservation reservation)
        {
            
            if (ModelState.IsValid)
            {
                var data_r= db.reservation.Where(x => x.id_user == reservation.id_user && x.id_abn==reservation.id_abn).FirstOrDefault();
                if (data_r != null)
                {
                    ViewBag.Notification = "User already Have This Subscription !!";
                    ViewBag.id_abn = new SelectList(db.abonnement, "id_abn", "ville_depart", reservation.id_abn);
                    ViewBag.id_user = new SelectList(db.users, "id_user", "nom_complet", reservation.id_user);
                    return View(reservation);
                }
                if (reservation.date_res == null)
                {
                    ViewBag.Notification = "Please Choose a Date !!";
                    ViewBag.id_abn = new SelectList(db.abonnement, "id_abn", "ville_depart", reservation.id_abn);
                    ViewBag.id_user = new SelectList(db.users, "id_user", "nom_complet", reservation.id_user);
                    return View(reservation);                  
                }
                if (DateTime.Compare((DateTime)DateTime.Now, (DateTime)reservation.date_res) == 1)
                {
                    ViewBag.Notification = "Please Choose a date from the future !!";
                    ViewBag.id_abn = new SelectList(db.abonnement, "id_abn", "ville_depart", reservation.id_abn);
                    ViewBag.id_user = new SelectList(db.users, "id_user", "nom_complet", reservation.id_user);
                    return View(reservation);
                }
                db.reservation.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_abn = new SelectList(db.abonnement, "id_abn", "ville_depart", reservation.id_abn);
            ViewBag.id_user = new SelectList(db.users, "id_user", "nom_complet", reservation.id_user);
            return View(reservation);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        [Route("ajouter_u")]
        public ActionResult Create(reservation reservation,users user)
        {
            
            if(user.lgn!=null)
            {
                reservation.date_res = DateTime.Now;
                var dataItem_u = db.users.Where(x => x.lgn == user.lgn).First();
                reservation.users = dataItem_u;
                reservation.id_user = dataItem_u.id_user;
                var dataItem_a = db.abonnement.Where(x => x.id_abn == reservation.id_abn).First();
                reservation.abonnement = dataItem_a;
                db.reservation.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index", "abonnements");

            }
            return RedirectToAction("Index", "abonnements");
        }

        

        // GET: reservations/Delete/5
        [Route("suppr/{id}")]
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reservation reservation = db.reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("suppr/{id}")]
        
        public ActionResult DeleteConfirmed(int id)
        {
            reservation reservation = db.reservation.Find(id);
            db.reservation.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
