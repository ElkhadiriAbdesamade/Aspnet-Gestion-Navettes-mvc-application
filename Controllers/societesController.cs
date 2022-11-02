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
    [RoutePrefix("societes")]
    [Authorize(Roles = "Admin")]
    public class societesController : Controller
    {
        private Gestion_NavettesEntities db = new Gestion_NavettesEntities();

        [Route()]
        // GET: societes
        public ActionResult Index()
        {
            if (TempData["del_status"] == null || TempData["del_status"].ToString() == "")
            {
                ViewBag.Notification = "";
                return View(db.societe.ToList());
            }
            ViewBag.Notification =  TempData["del_status"].ToString();
            return View(db.societe.ToList());
        }

        // GET: societes/Details/5
        [Route("details/{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            societe societe = db.societe.Find(id);
            if (societe == null)
            {
                return HttpNotFound();
            }
            return View(societe);
        }

        // GET: societes/Create
        [Route("ajouter")]
        public ActionResult Create()
        {
            
                ViewBag.Notification = "";
            
            return View();
        }

        // POST: societes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajouter")]
        public ActionResult Create([Bind(Include = "id_soc,nom_soc,adrs_soc,ville_soc")] societe societe)
        {

            if (societe.nom_soc == null || societe.adrs_soc == null || societe.ville_soc == null)
            {
                ViewBag.Notification = "Please Enter Sosiety Info  !!";
                return View(societe);
            }
            var soc = db.societe.Where(x => x.nom_soc == societe.nom_soc).FirstOrDefault();
            if (soc != null)
            {
                ViewBag.Notification = "Sosiety already Existed  !!";
                return View(societe);
            }
           

            if (ModelState.IsValid)
            {
                db.societe.Add(societe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(societe);
        }

        // GET: societes/Edit/5
        [Route("modifier/{id}")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Notification = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            societe societe = db.societe.Find(id);
            if (societe == null)
            {
                return HttpNotFound();
            }
            return View(societe);
        }

        // POST: societes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("modifier/{id}")]
        public ActionResult Edit([Bind(Include = "id_soc,nom_soc,adrs_soc,ville_soc")] societe societe)
        {
            if (societe.nom_soc == null || societe.adrs_soc == null || societe.ville_soc == null)
            {
                ViewBag.Notification = "Please Enter Sosiety Info  !!";
                return View(societe);
            }
            
            if (ModelState.IsValid)
            {
                db.Entry(societe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(societe);
        }

        // GET: societes/Delete/5
        [Route("suppr/{id}")]
        public ActionResult Delete(int? id)
        {
          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            societe societe = db.societe.Find(id);
            if (societe == null)
            {
                return HttpNotFound();
            }
            return View(societe);
        }

        // POST: societes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("suppr/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            var dataItem_a = db.abonnement.Where(x => x.id_soc == id).FirstOrDefault();
            if (dataItem_a!=null)
            {
                TempData["del_status"] = "You Can't Delete this Row Because it's used in a Subscription !!";
                return RedirectToAction("Index");
            }
            societe societe = db.societe.Find(id);
            db.societe.Remove(societe);
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
