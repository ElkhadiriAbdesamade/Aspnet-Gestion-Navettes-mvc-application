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
    [RoutePrefix("users")]
    [Authorize(Roles = "Admin")]
    public class usersController : Controller
    {
        private Gestion_NavettesEntities db = new Gestion_NavettesEntities();

        // GET: users
        [Route()]
        public ActionResult Index()
        {
            if (TempData["del_status"] == null || TempData["del_status"].ToString() == "")
            {
                ViewBag.Notification = "";
                return View(db.users.ToList());
            }
            ViewBag.Notification = TempData["del_status"].ToString();
            return View(db.users.ToList());
        }

        // GET: users/Details/5
        [Route("details/{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: users/Create
       /* [Route("ajouter")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajouter")]
        public ActionResult Create([Bind(Include = "id_user,nom_complet,email,tele,lgn,psw,roles")] users users)
        {
            if (ModelState.IsValid)
            {
                if (users.roles != "Admin" && users.roles != "User" && users.roles == null)
                {
                    ViewBag.Notification = "Role Not Valide !!";
                    return View(users);
                }
                db.users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(users);
        }*/

        // GET: users/Edit/5
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
            users users = db.users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("modifier/{id}")]
        public ActionResult Edit([Bind(Include = "id_user,nom_complet,email,tele,lgn,psw,roles")] users users)
        {
            if (users.nom_complet==null||users.email==null || users.tele == null || users.lgn == null || users.psw == null || users.roles == null)
            {
                ViewBag.Notification = "Please Enter User Info  !!";
                return View(users);
            }
            

            if (ModelState.IsValid)
            {
                if (users.roles != "Admin" && users.roles != "User" && users.roles == null)
                {
                    ViewBag.Notification = "Role Not Valide !!";
                    return View(users);
                }
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: users/Delete/5
        [Route("suppr/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("suppr/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            var dataItem_a = db.reservation.Where(x => x.id_user == id).FirstOrDefault();
            var dataItem_d = db.demande_user.Where(x => x.id_user == id).FirstOrDefault();
            if (dataItem_a != null || dataItem_d != null)
            {
                TempData["del_status"] = "You Can't Delete this User Because He Reserve a Subscription !!";
                return RedirectToAction("Index");
            }

            users users = db.users.Find(id);
            db.users.Remove(users);
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
