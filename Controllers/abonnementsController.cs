using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gestion_Navettes.Models;

namespace Gestion_Navettes.Controllers
{
    
   
    public class abonnementsController : Controller
    {
        private Gestion_NavettesEntities db = new Gestion_NavettesEntities();

       
       
        [Route()]
        // GET: abonnements
        public ActionResult Index()
        {
            if (TempData["status"]==null)
            {
                TempData["status"] = "";
            }
            
            var abonnement = db.abonnement.Include(a => a.societe).Include(a => a.vehicule);            
            return View(abonnement.ToList());
        }

        // GET: abonnements/Details/5
        [Authorize(Roles = "Admin")]
        [Route("details/{id}")]
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            abonnement abonnement = db.abonnement.Find(id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            return View(abonnement);
        }

        [Authorize(Roles = "Admin")]
        [Route("ajouter")]
        // GET: abonnements/Create
        public ActionResult Create(abonnement a)
        {
            if (ViewBag.Notification == null)
            {
                ViewBag.Notification = "";
            }

            ViewBag.ville_depart = a.ville_depart;
            ViewBag.ville_arrive = a.ville_arrive;
            ViewBag.date_depart = a.date_depart.ToShortDateString();
            ViewBag.date_arrive = a.date_arrive.ToShortDateString();
                      
            ViewBag.id_soc = new SelectList(db.societe, "id_soc", "nom_soc");
            ViewBag.id_vh = new SelectList(db.vehicule, "id_vh", "nom_vh");
            return View();
            
        }

        // POST: abonnements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("ajouter")]
        public ActionResult Create(abonnement abonnement, HttpPostedFileBase img)
        {

            if (abonnement.ville_depart==null || abonnement.ville_arrive == null || abonnement.date_depart == null || abonnement.date_arrive == null || abonnement.prix == null || abonnement.heur_depart == null || abonnement.heur_arrive == null || abonnement.descr == null)            
            {
                ViewBag.Notification = "Please Enter Subscription Info  !!";
                ViewBag.id_soc = new SelectList(db.societe, "id_soc", "nom_soc", abonnement.id_soc);
                ViewBag.id_vh = new SelectList(db.vehicule, "id_vh", "nom_vh", abonnement.id_vh);
                return View(abonnement);
            }
            var abn = db.abonnement.Where(x => x.ville_depart == abonnement.ville_depart && x.ville_arrive==abonnement.ville_arrive && x.date_depart == abonnement.date_depart && x.date_arrive == abonnement.date_arrive).FirstOrDefault();
            if (abn != null)
            {
                ViewBag.Notification = "Sosiety Subscription Existed  !!";                
                ViewBag.id_soc = new SelectList(db.societe, "id_soc", "nom_soc", abonnement.id_soc);
                ViewBag.id_vh = new SelectList(db.vehicule, "id_vh", "nom_vh", abonnement.id_vh);
                return View(abonnement);
            }

            if (img != null)
            {
                //Use Namespace called :  System.IO  
                string FileName = Path.GetFileNameWithoutExtension(img.FileName);
                //To Get File Extension  
                string FileExtension = Path.GetExtension(img.FileName);

                //Add Current Date To Attached File Name  
                FileName = DateTime.Now.ToString("dd-mm-ss") + FileExtension;

                //Get Upload path from Web.Config file AppSettings.  
                string UploadPath = "~/Images/";  //ConfigurationManager.AppSettings["pieces_jointes_Path"].ToString();
                                                  //Its Create complete path to store in server.  
                abonnement.abn_image = FileName; //UploadPath +
                                                 //To copy and save file into server.  
                img.SaveAs(Server.MapPath(UploadPath + FileName));
            }
            else
            {
                abonnement.abn_image = "none.png";
            }
               
                db.abonnement.Add(abonnement);
                db.SaveChanges();
                return RedirectToAction("Index");
            
            
             ViewBag.id_soc = new SelectList(db.societe, "id_soc", "nom_soc", abonnement.id_soc);
            ViewBag.id_vh = new SelectList(db.vehicule, "id_vh", "nom_vh", abonnement.id_vh);
            return View(abonnement);
        }

        // GET: abonnements/Edit/5
        [Authorize(Roles = "Admin")]
        [Route("modifier/{id}")]
        public ActionResult Edit(int? id)
        {
            if (ViewBag.Notification == null)
            {
                ViewBag.Notification = "";
            }
            //Session["role"] = getsession();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            abonnement abonnement = db.abonnement.Find(id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_soc = new SelectList(db.societe, "id_soc", "nom_soc", abonnement.id_soc);
            ViewBag.id_vh = new SelectList(db.vehicule, "id_vh", "nom_vh", abonnement.id_vh);
            return View(abonnement);
        }

        // POST: abonnements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("modifier/{id}")]
        public ActionResult Edit(abonnement abonnement, HttpPostedFileBase img)
        {
            if (abonnement.ville_depart == null || abonnement.ville_arrive == null || abonnement.date_depart == null || abonnement.date_arrive == null || abonnement.prix == null || abonnement.heur_depart == null || abonnement.heur_arrive == null || abonnement.descr == null)
            {
                ViewBag.Notification = "Please Enter Subscription Info  !!";
                ViewBag.id_soc = new SelectList(db.societe, "id_soc", "nom_soc", abonnement.id_soc);
                ViewBag.id_vh = new SelectList(db.vehicule, "id_vh", "nom_vh", abonnement.id_vh);
                return View(abonnement);
            }

            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    //Use Namespace called :  System.IO  
                    string FileName = Path.GetFileNameWithoutExtension(img.FileName);
                    //To Get File Extension  
                    string FileExtension = Path.GetExtension(img.FileName);

                    //Add Current Date To Attached File Name  
                    FileName = DateTime.Now.ToString("dd-mm-ss") + FileExtension;

                    //Get Upload path from Web.Config file AppSettings.  
                    string UploadPath = "~/Images/";  //ConfigurationManager.AppSettings["pieces_jointes_Path"].ToString();
                                                      //Its Create complete path to store in server.  
                    abonnement.abn_image = FileName; //UploadPath +
                                                     //To copy and save file into server.  
                    img.SaveAs(Server.MapPath(UploadPath + FileName));
                }

                db.Entry(abonnement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_soc = new SelectList(db.societe, "id_soc", "nom_soc", abonnement.id_soc);
            ViewBag.id_vh = new SelectList(db.vehicule, "id_vh", "nom_vh", abonnement.id_vh);
            return View(abonnement);
        }

        // GET: abonnements/Delete/5
        [Authorize(Roles = "Admin")]
        [Route("suppr/{id}")]
        public ActionResult Delete(int? id)
        {           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            abonnement abonnement = db.abonnement.Find(id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            return View(abonnement);
        }

        // POST: abonnements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("suppr/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {            
            abonnement abonnement = db.abonnement.Find(id);
            db.abonnement.Remove(abonnement);
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
