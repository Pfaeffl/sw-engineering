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
    public class GoodsReceiptsController : BaseController
    {
         private class OrderOrError
        {
            public ActionResult Error;
            public GoodsReceipt GoodsReceipt;
            public OrderOrError(ActionResult error)
            {
                Error = error;
            }
            public OrderOrError(HttpStatusCode errorStatus)
            {
                Error =
                    new HttpStatusCodeResult(errorStatus);
            }
            public OrderOrError(GoodsReceipt goodsReceipt)
            {
                GoodsReceipt = goodsReceipt;
            }
        }
        private OrderOrError GetOrderOrError(
            GoodsReceipt receipt)
        {
            if (!hasUser())
                return new
                    OrderOrError(HttpStatusCode.Unauthorized);
            if (receipt.MaterialId as int? == null)
                return new
                    OrderOrError(HttpStatusCode.BadRequest);

            Material material = db.Materials.Find(receipt.MaterialId);
            if (material == null)
                return new
                    OrderOrError(HttpStatusCode.BadRequest);

            receipt.Mat = material;

            return new OrderOrError(receipt);
        }


        // GET: GoodsReceipts/Create
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

        // POST: GoodsReceipts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Wareneingang,MaterialId")] GoodsReceipt goodsReceipt, int? materialId)
        {
            ViewBag.Name = getName();
            ViewBag.Lagerist = getRoleLagerist();
            ViewBag.Systemadm = getRoleSystemadm();
            var tmp = GetOrderOrError(goodsReceipt);
            if (tmp.Error != null) return tmp.Error;
            if (tmp.GoodsReceipt.Wareneingang <= 0)
            {
                ModelState.AddModelError("1", "Buchung nicht möglich: Die WE-Menge muss größer 0 sein.");
                ViewBag.MaterialId =
                new SelectList(
                    db.Materials.Where(material => material.Id == materialId), "Id", "Materialnummer");
                return View();
            }
            if (ModelState.IsValid)
            {
                tmp.GoodsReceipt.Mat.Bestand = tmp.GoodsReceipt.Mat.Bestand + tmp.GoodsReceipt.Wareneingang;
                db.SaveChanges();
                return RedirectToAction("Index", "Lager");
            }
            return View(goodsReceipt);
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
