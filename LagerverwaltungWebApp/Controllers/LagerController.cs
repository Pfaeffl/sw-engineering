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
    public class LagerController : BaseController
    {          

        // GET: Lager
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
            var materials = db.Materials.Include(m => m.Lagerort);
            return View(materials.ToList());
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
