using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LagerverwaltungLib;
using LagerverwaltungWebApp.Models;

namespace LagerverwaltungWebApp.Controllers
{
    public class GoodsIssuesController : BaseController
    {
        private class OrderOrError
        {
            public ActionResult Error;
            public GoodsIssue GoodsIssue;
            public OrderOrError(ActionResult error)
            {
                Error = error;
            }
            public OrderOrError(HttpStatusCode errorStatus)
            {
                Error =
                    new HttpStatusCodeResult(errorStatus);
            }
            public OrderOrError(GoodsIssue goodsIssue)
            {
                GoodsIssue = goodsIssue;
            }
        }
        private OrderOrError GetOrderOrError(
            GoodsIssue issue)
        {
            if (!hasUser())
                return new
                    OrderOrError(HttpStatusCode.Unauthorized);
            if (issue.MaterialId as int? == null)
                return new
                    OrderOrError(HttpStatusCode.BadRequest);

            Material material = db.Materials.Find(issue.MaterialId);
            if (material == null)
                return new
                    OrderOrError(HttpStatusCode.BadRequest);

            issue.Mat = material;

            return new OrderOrError(issue);
        }

        // GET: GoodsIssues/Create
        public ActionResult Create(int? materialId)
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

            ViewBag.MaterialId =
                new SelectList(
                    db.Materials.Where(material => material.Id == materialId), "Id", "Materialnummer");
            return View();
        }

        // POST: GoodsIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Warenausgang,MaterialId")] GoodsIssue goodsIssue, int? materialId)
        {
            ViewBag.Name = getName();
            ViewBag.Lagerist = getRoleLagerist();
            ViewBag.Systemadm = getRoleSystemadm();
            var tmp = GetOrderOrError(goodsIssue);
            if (tmp.Error != null) return tmp.Error;
            if (tmp.GoodsIssue.Warenausgang <= 0)
            {
                ModelState.AddModelError("1", "Buchung nicht möglich: Die WA-Menge muss größer 0 sein.");
                ViewBag.MaterialId =
                new SelectList(
                    db.Materials.Where(material => material.Id == materialId), "Id", "Materialnummer");
                return View();
            }
            if (tmp.GoodsIssue.Mat.Bestand - tmp.GoodsIssue.Warenausgang < 0)
            {
                int x = tmp.GoodsIssue.Warenausgang - tmp.GoodsIssue.Mat.Bestand;
                ModelState.AddModelError("2", "Buchung nicht möglich: Der Bestand wurde um " + x +
                    " Stück unterschritten. Es können max. " + tmp.GoodsIssue.Mat.Bestand + " Stück entnommen werden.");
                ViewBag.MaterialId =
                new SelectList(
                    db.Materials.Where(material => material.Id == materialId), "Id", "Materialnummer");
                return View();
            }
            if (ModelState.IsValid)
            {
                tmp.GoodsIssue.Mat.Bestand = tmp.GoodsIssue.Mat.Bestand - tmp.GoodsIssue.Warenausgang;
                db.SaveChanges();
                return RedirectToAction("Index", "Lager");
            }
            return View(goodsIssue);
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
