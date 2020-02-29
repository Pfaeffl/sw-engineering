using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using LagerverwaltungLib;

namespace LagerverwaltungWebApp.Controllers
{
    public class MyMaterialsController : BaseController
    {
        
        // GET: MyMaterials
        public ActionResult Index()
        {
            ViewBag.Name = getName();
            ViewBag.Besteller = getRoleBesteller();
            ViewBag.Systemadm = getRoleSystemadm();
            if (!hasUser())
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.BadRequest);
            }
            if (LoggedInUser.Besteller == false && LoggedInUser.Systemadm == false)
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.Unauthorized);
            }
            if (LoggedInUser.Systemadm == true)
            {
                var mat = db.Materials.Include(m => m.Lagerort);
                return View(mat.ToList());
            }
            var materials = db.Materials.Where(material => material.Users.Any(user => user.Id == LoggedInUser.Id)).Include(m => m.Lagerort);
            return View(materials.ToList());
            
        }

        // GET: MyMaterials/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Name = getName();
            ViewBag.Besteller = getRoleBesteller();
            ViewBag.Systemadm = getRoleSystemadm();
            if (!hasUser())
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.BadRequest);
            }
            if (LoggedInUser.Besteller == false && LoggedInUser.Systemadm == false)
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.Unauthorized);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }

        // GET: MyMaterials/Create
        public ActionResult Create()
        {
            ViewBag.Name = getName();
            ViewBag.Besteller = getRoleBesteller();
            ViewBag.Systemadm = getRoleSystemadm();
            if (!hasUser())
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.BadRequest);
            }
            if (LoggedInUser.Besteller == false && LoggedInUser.Systemadm == false)
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.Unauthorized);
            }
            ViewBag.LagerortId = new SelectList(db.Lagerorts, "Id", "Name");
            return View();
        }

        // POST: MyMaterials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LagerortId,Materialnummer,Materialbezeichnung,Standardpreis,Bestand,Mindestbestand,Bestellmenge")] Material material)
        {
                       
            if (ModelState.IsValid)
            {
                if (!hasUser())
                {
                    return new HttpStatusCodeResult(
                            HttpStatusCode.BadRequest);
                }
                db.Materials.Add(material);
                material.Users.Add(LoggedInUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LagerortId = new SelectList(db.Lagerorts, "Id", "Name", material.LagerortId);
            return View(material);
        }

        // GET: MyMaterials/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Name = getName();
            ViewBag.Besteller = getRoleBesteller();
            ViewBag.Systemadm = getRoleSystemadm();
            if (!hasUser())
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.BadRequest);
            }
            if (LoggedInUser.Besteller == false && LoggedInUser.Systemadm == false)
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.Unauthorized);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            ViewBag.LagerortId = new SelectList(db.Lagerorts, "Id", "Name", material.LagerortId);
            return View(material);
        }

        // POST: MyMaterials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LagerortId,Materialnummer,Materialbezeichnung,Standardpreis,Bestand,Mindestbestand,Bestellmenge")] Material material)
        {
            if (ModelState.IsValid)
            {
                db.Entry(material).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LagerortId = new SelectList(db.Lagerorts, "Id", "Name", material.LagerortId);
            return View(material);
        }

        // GET: MyMaterials/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Name = getName();
            ViewBag.Besteller = getRoleBesteller();
            ViewBag.Systemadm = getRoleSystemadm();
            if (!hasUser())
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.BadRequest);
            }
            if (LoggedInUser.Besteller == false && LoggedInUser.Systemadm == false)
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.Unauthorized);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }

        // POST: MyMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Material material = db.Materials.Find(id);
            db.Materials.Remove(material);
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
