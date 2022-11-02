using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gestion_Navettes.Models;

namespace Gestion_Navettes.Controllers
{
    [RoutePrefix("Vehicules")]
    [Authorize(Roles = "Admin")]
    public class vehiculesController : Controller
    {
        private Gestion_NavettesEntities db = new Gestion_NavettesEntities();

        // GET: vehicules
        [Route()]
        public ActionResult Index()
        {
            if (TempData["del_status"] == null || TempData["del_status"].ToString() == "")
            {
                ViewBag.Notification = "";
                return View(db.vehicule.ToList());
            }
            ViewBag.Notification = TempData["del_status"].ToString();
            return View(db.vehicule.ToList());
        }

        // GET: vehicules/Details/5
        [Route("details/{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vehicule vehicule = db.vehicule.Find(id);
            if (vehicule == null)
            {
                return HttpNotFound();
            }
            return View(vehicule);
        }

        // GET: vehicules/Create
        [Route("ajouter")]
        public ActionResult Create()
        {
            if (ViewBag.Notification == null)
            {
                ViewBag.Notification = "";
            }
            return View();
        }

        // POST: vehicules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajouter")]
        public ActionResult Create([Bind(Include = "id_vh,nom_vh,capacite_vh,marque_vh,modele_vh,immatricul_vh")] vehicule vehicule)
        {
            if (vehicule.nom_vh == null || vehicule.capacite_vh == 0 || vehicule.marque_vh == null || vehicule.modele_vh == null || vehicule.immatricul_vh == null)
            {
                ViewBag.Notification = "Please Enter vehicule Info  !!";
                return View(vehicule);
            }
            var vh = db.societe.Where(x => x.nom_soc == vehicule.nom_vh).FirstOrDefault();
            if (vh != null)
            {
                ViewBag.Notification = "vehicule already Existed  !!";
                return View(vehicule);
            }

            if (ModelState.IsValid)
            {
                db.vehicule.Add(vehicule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vehicule);
        }

        // GET: vehicules/Edit/5
        [Route("modifier/{id}")]
        public ActionResult Edit(int? id)
        {
            if (ViewBag.Notification == null)
            {
                ViewBag.Notification = "";
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vehicule vehicule = db.vehicule.Find(id);
            if (vehicule == null)
            {
                return HttpNotFound();
            }
            return View(vehicule);
        }

        // POST: vehicules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("modifier/{id}")]
        public ActionResult Edit([Bind(Include = "id_vh,nom_vh,capacite_vh,marque_vh,modele_vh,immatricul_vh")] vehicule vehicule)
        {
            if (vehicule.nom_vh == null || vehicule.capacite_vh == 0 || vehicule.marque_vh == null || vehicule.modele_vh == null || vehicule.immatricul_vh == null)
            {
                ViewBag.Notification = "Please Enter vehicule Info  !!";
                return View(vehicule);
            }

            if (ModelState.IsValid)
            {
                db.Entry(vehicule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicule);
        }

        // GET: vehicules/Delete/5
        [Route("suppr/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vehicule vehicule = db.vehicule.Find(id);
            if (vehicule == null)
            {
                return HttpNotFound();
            }
            return View(vehicule);
        }

        // POST: vehicules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("suppr/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            var dataItem_a = db.abonnement.Where(x => x.id_vh == id).FirstOrDefault();
            if (dataItem_a != null)
            {
                TempData["del_status"] = "You Can't Delete this Row Because it's used in a Subscription !!";
                return RedirectToAction("Index");
            }

            vehicule vehicule = db.vehicule.Find(id);
            db.vehicule.Remove(vehicule);
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
