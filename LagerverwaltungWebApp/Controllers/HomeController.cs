using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LagerverwaltungWebApp.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Name = getName();
            return View();
        }

        public ActionResult Lager()
        {
            ViewBag.Name = getName();
            ViewBag.Lagerist = getRoleLagerist();
            return View();
        }

        public ActionResult Einkauf()
        {
            ViewBag.Name = getName();
            ViewBag.Besteller = getRoleBesteller();
            return View();
        }

        public ActionResult Admin()
        {
            ViewBag.Name = getName();
            ViewBag.Systemadm = getRoleSystemadm();
            return View();
        }

        public ActionResult Personal()
        {
            ViewBag.Name = getName();
            ViewBag.Personal = getRolePersonal();
            return View();
        }

    }
}