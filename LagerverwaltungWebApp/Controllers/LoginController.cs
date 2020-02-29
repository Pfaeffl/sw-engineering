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
    public class LoginController : BaseController
    {
        
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.Name = getName();
            ViewBag.Lagerist = getRoleLagerist();
            ViewBag.Besteller = getRoleBesteller();
            ViewBag.Systemadm = getRoleSystemadm();
            ViewBag.Personal = getRolePersonal();
            return View(db.Users.ToList());
        }

        // GET: Login/Details/5
        public ActionResult Select(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            setUserId(id.Value);
            if (LoggedInUser.Lagerist == true)
            {
                return RedirectToAction("Lager", "Home");
            }
            if (LoggedInUser.Besteller == true)
            {
                return RedirectToAction("Einkauf", "Home");
            }
            if (LoggedInUser.Personal == true)
            {
                return RedirectToAction("Personal", "Home");
            }
            if (LoggedInUser.Systemadm == true)
            {
                return RedirectToAction("Admin", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            resetUserId();
            return RedirectToAction("Index", "Home");
        }
    }
}
