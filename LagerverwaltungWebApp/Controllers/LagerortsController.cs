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
    public class LagerortsController : BaseController
    {
        
        // GET: Lagerorts
        public ActionResult Index()
        {
            ViewBag.Name = getName();
            ViewBag.Lagerist = getRoleLagerist();
            ViewBag.Systemadm = getRoleSystemadm();
            if (!hasUser())
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.BadRequest);
            }
            if (LoggedInUser.Lagerist == false && LoggedInUser.Systemadm == false)
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.Unauthorized);
            }
            return View(db.Lagerorts.ToList());
        }

        // GET: Lagerorts/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Name = getName();
            ViewBag.Lagerist = getRoleLagerist();
            ViewBag.Systemadm = getRoleSystemadm();
            if (!hasUser())
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.BadRequest);
            }
            if (LoggedInUser.Lagerist == false && LoggedInUser.Systemadm == false)
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.Unauthorized);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lagerort lagerort = db.Lagerorts.Find(id);
            if (lagerort == null)
            {
                return HttpNotFound();
            }
            return View(lagerort);
        }

        // GET: Lagerorts/Create
        public ActionResult Create()
        {
            ViewBag.Name = getName();
            ViewBag.Lagerist = getRoleLagerist();
            ViewBag.Systemadm = getRoleSystemadm();
            if (!hasUser())
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.BadRequest);
            }
            if (LoggedInUser.Lagerist == false && LoggedInUser.Systemadm == false)
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.Unauthorized);
            }
            return View();
        }

        // POST: Lagerorts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Lagerort lagerort)
        {
            if (ModelState.IsValid)
            {
                db.Lagerorts.Add(lagerort);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lagerort);
        }

        // GET: Lagerorts/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Name = getName();
            ViewBag.Lagerist = getRoleLagerist();
            ViewBag.Systemadm = getRoleSystemadm();
            if (!hasUser())
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.BadRequest);
            }
            if (LoggedInUser.Lagerist == false && LoggedInUser.Systemadm == false)
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.Unauthorized);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lagerort lagerort = db.Lagerorts.Find(id);
            if (lagerort == null)
            {
                return HttpNotFound();
            }
            return View(lagerort);
        }

        // POST: Lagerorts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Lagerort lagerort)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lagerort).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lagerort);
        }

        // GET: Lagerorts/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Name = getName();
            ViewBag.Lagerist = getRoleLagerist();
            ViewBag.Systemadm = getRoleSystemadm();
            if (!hasUser())
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.BadRequest);
            }
            if (LoggedInUser.Lagerist == false && LoggedInUser.Systemadm == false)
            {
                return new HttpStatusCodeResult(
                        HttpStatusCode.Unauthorized);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lagerort lagerort = db.Lagerorts.Find(id);
            if (lagerort == null)
            {
                return HttpNotFound();
            }
            return View(lagerort);
        }

        // POST: Lagerorts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lagerort lagerort = db.Lagerorts.Find(id);
            db.Lagerorts.Remove(lagerort);
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
