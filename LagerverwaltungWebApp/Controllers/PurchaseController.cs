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
    public class PurchaseController : BaseController
    {
        // GET: Purchase
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
                var mat = db.Materials.Where(material => material.Bestand < material.Mindestbestand).Include(m => m.Lagerort);
                return View(mat.ToList());
            }
            var materials = db.Materials.Where(material => material.Users.Any(user => user.Id == LoggedInUser.Id)).Where(material => material.Bestand < material.Mindestbestand).Include(m => m.Lagerort);
            return View(materials.ToList());
        }

        public ActionResult Bestellen(int? materialId)
        {
            ViewBag.Name = getName();
            ViewBag.Besteller = getRoleBesteller();
            ViewBag.Systemadm = getRoleSystemadm();
            if (materialId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(materialId);
            if (material == null)
            {
                return HttpNotFound();
            }
            ViewBag.Message = "Von Material " + material.Materialnummer + " - " + material.Materialbezeichnung + " wurden " + material.Bestellmenge + " Stück nachbestellt.";
                return View();
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
