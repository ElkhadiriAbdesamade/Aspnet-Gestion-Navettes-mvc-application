using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gestion_Navettes.Models;
using OpenXmlPowerTools;

namespace Gestion_Navettes.Controllers
{
    [RoutePrefix("demande")]
    public class demande_abnController : Controller
    {
        private Gestion_NavettesEntities db = new Gestion_NavettesEntities();

        // GET: demande_abn
        [Authorize(Roles = "Admin")]
        [Route()]
        public ActionResult Index()
        {
            var demande_abn = db.demande_abn;
            ViewBag.id_users = new SelectList(db.users, "id_users", "nom_complet");
            return View(demande_abn.ToList());
        }
        [Authorize(Roles = "User")]
        [Route("MyRequest")]
        public ActionResult Index(users user)
        {            
            String lgn = HttpContext.User.Identity.Name;
            var dataItem_u = db.users.Where(x => x.lgn == lgn).First();
            var dataItem_d_u=db.demande_user.Where(x => x.id_user == dataItem_u.id_user).FirstOrDefault();
            if (dataItem_d_u==null)
            {
                TempData["status"] = "You Don't Have any Request !!";
                return RedirectToAction("Index", "abonnements") ;
            }
            var demande = db.demande_abn.Where(x => x.id_dm == dataItem_d_u.id_dm);
            return View(demande.ToList());
        }


       

      

        // POST: demande_abn/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        [Route("ajouter_u")]
        public ActionResult Create(demande_abn demande_abn,demande_user demande_user)
        {
            if (demande_abn.ville_depart == null || demande_abn.ville_arrive == null || demande_abn.date_depart == null || demande_abn.date_arrive == null)
            {
                TempData["status"] = "Please Enter The Request Info !!";
                return RedirectToAction("Index", "abonnements");
            }
            var abn = db.abonnement.Where(x => x.ville_depart == demande_abn.ville_depart && x.ville_arrive == demande_abn.ville_arrive).FirstOrDefault();
            if (abn != null)
            {
                TempData["status"] = "We already Have This Subscription !!";
                return RedirectToAction("Index", "abonnements");
            }
            String lgn = HttpContext.User.Identity.Name;
            var dataItem_u = db.users.Where(x => x.lgn == lgn).First();
            var dataItem_d_u = db.demande_user.Where(x => x.id_user == dataItem_u.id_user).FirstOrDefault();
            var dataItem_d = db.demande_abn.Where(x => x.ville_arrive == demande_abn.ville_arrive && x.ville_depart == demande_abn.ville_depart).FirstOrDefault();
            if (dataItem_d_u==null)
            {
                if (dataItem_d !=null)
                {
                    //Update nbr de Demnde
                    dataItem_d.nbr_dm += 1;
                    db.Entry(dataItem_d).State = EntityState.Modified;
                    db.SaveChanges();

                    //Add User To List de demande
                    demande_user.id_user = dataItem_u.id_user;
                    demande_user.id_dm = dataItem_d.id_dm;
                    db.demande_user.Add(demande_user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "abonnements");
                }
                else
                {
                    

                    if (DateTime.Compare(DateTime.Now, (DateTime)demande_abn.date_depart) == 1)
                    {
                        TempData["status"] = "Departure Date Must be from the future !!";
                        return RedirectToAction("Index", "abonnements");
                    }
                    if (DateTime.Compare(DateTime.Now, (DateTime)demande_abn.date_arrive) == 1)
                    {
                        TempData["status"] = "Arrival Date Must be from the future !!";
                        return RedirectToAction("Index", "abonnements");
                    }
                    if (DateTime.Compare((DateTime)demande_abn.date_depart, (DateTime)demande_abn.date_arrive) == 1)
                    {
                        TempData["status"] = "Arrival Date Must be Greater than Departure Date!!";
                        return RedirectToAction("Index", "abonnements");
                    }
                    //Creation de La Demande
                    demande_abn.nbr_dm = 1;
                    db.demande_abn.Add(demande_abn);
                    db.SaveChanges();

                    //Add User To List de demande
                    demande_user.date_d = DateTime.Now;
                    demande_user.id_user = dataItem_u.id_user;
                    demande_user.id_dm = demande_abn.id_dm;
                    db.demande_user.Add(demande_user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "abonnements");
                }
            }
            var data_dm = db.demande_user.Where(x => x.id_user == dataItem_d_u.id_user && x.demande_abn.ville_arrive == demande_abn.ville_arrive && x.demande_abn.ville_depart == demande_abn.ville_depart).FirstOrDefault();
            if (data_dm != null)
            {
                TempData["status"] = "You Already Request This Subscription !!";
                return RedirectToAction("Index", "abonnements");
            }
            else
            {                
                if (DateTime.Compare(DateTime.Now, (DateTime)demande_abn.date_depart) == 1)
                {
                    TempData["status"] = "Departure Date Must be from the future !!";
                    return RedirectToAction("Index", "abonnements");
                }
                if (DateTime.Compare(DateTime.Now, (DateTime)demande_abn.date_arrive) == 1)
                {
                    TempData["status"] = "Arrival Date Must be from the future !!";
                    return RedirectToAction("Index", "abonnements");
                }
                if (DateTime.Compare((DateTime)demande_abn.date_depart, (DateTime)demande_abn.date_arrive) == 1)
                {
                    TempData["status"] = "Arrival Date Must be Greater than Departure Date!!";
                    return RedirectToAction("Index", "abonnements");
                }
                demande_abn.nbr_dm = 1;
                db.demande_abn.Add(demande_abn);
                db.SaveChanges();

                demande_user.date_d = DateTime.Now;
                demande_user.id_user = dataItem_u.id_user;
                demande_user.id_dm = demande_abn.id_dm;
                db.demande_user.Add(demande_user);
                db.SaveChanges();

                return RedirectToAction("Index", "abonnements");
            }
           
           
        }

        

      

        // GET: demande_abn/Delete/5
        [Route("suppr/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            demande_abn demande_abn = db.demande_abn.Find(id);
            if (demande_abn == null)
            {
                return HttpNotFound();
            }
            return View(demande_abn);
        }

        // POST: demande_abn/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("suppr/{id}")]        
        public ActionResult DeleteConfirmed(int id)
        {
            var dataItem_d = db.demande_abn.Find(id);
            demande_abn demande_abn;
            if (dataItem_d.nbr_dm>1)
            {
                var dataItem_d_us = db.demande_user.Where(x => x.id_dm == dataItem_d.id_dm);
                foreach (var item in dataItem_d_us.ToList())
                {
                    db.demande_user.Remove(item);
                    db.SaveChanges();
                }
                demande_abn = db.demande_abn.Find(id);
                db.demande_abn.Remove(demande_abn);
                db.SaveChanges();
                return RedirectToAction("Index");
                /* dataItem_d.nbr_dm -= 1;
                 db.Entry(dataItem_d).State = EntityState.Modified;
                 db.SaveChanges();
                 return RedirectToAction("Index");*/
            }
            var dataItem_d_u = db.demande_user.Where(x => x.id_dm == dataItem_d.id_dm).FirstOrDefault();
            db.demande_user.Remove(dataItem_d_u);
            db.SaveChanges();

            demande_abn = db.demande_abn.Find(id);
            db.demande_abn.Remove(demande_abn);
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
