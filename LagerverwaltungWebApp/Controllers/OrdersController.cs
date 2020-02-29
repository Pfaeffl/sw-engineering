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
    public class OrdersController : BaseController
    {
        private class OrderOrError
        {
            public ActionResult Error;
            public Order Order;
            public OrderOrError(ActionResult error)
            {
                Error = error;
            }
            public OrderOrError(HttpStatusCode errorStatus)
            {
                Error =
                    new HttpStatusCodeResult(errorStatus);
            }
            public OrderOrError(Order order)
            {
                Order = order;
            }
        }
        private OrderOrError GetOrderOrError(
            int? userId, int? materialId)
        {
            if (!hasUser())
                return new
                    OrderOrError(HttpStatusCode.Unauthorized);
            if (userId == null)
                return new
                    OrderOrError(HttpStatusCode.BadRequest);
            if (materialId == null)
                return new
                    OrderOrError(HttpStatusCode.BadRequest);

            Material material = db.Materials.Find(materialId);
            if (material == null)
                return new
                    OrderOrError(HttpStatusCode.BadRequest);
            User user = db.Users.Find(userId);
            if (user == null)
                return new
                    OrderOrError(HttpStatusCode.BadRequest);
            return new OrderOrError(new Order()
            { User = user, Material = material });
        }

    // GET: Orders
    public ActionResult Index()
        {
            ViewBag.Name = getName();
            ViewBag.Besteller = getRoleBesteller();
            ViewBag.Systemadm = getRoleSystemadm();
            var orders =
            from user in db.Users
            from material in db.Materials
            where user.Materials.Contains(material)
            select new Order()
            { User = user, Material = material };
                   
            return View(orders.ToList());
        }

        // GET: Orders/Create
        public ActionResult Create(int? materialId)
        {
            ViewBag.Name = getName();
            ViewBag.Besteller = getRoleBesteller();
            ViewBag.Systemadm = getRoleSystemadm();
            ViewBag.UserId =
                new SelectList(
                    db.Users.Where(user => !user.Materials.Any(material => material.Id == materialId))
                    .Where(user => user.Besteller == true), "Id", "Name");
            ViewBag.MaterialId =
                new SelectList(
                    db.Materials.Where(material => material.Id == materialId), "Id", "Materialnummer");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,MaterialId")] Order order)
        {
            var tmp = GetOrderOrError(
                    order.UserId, order.MaterialId);
            if (tmp.Error != null) return tmp.Error;
            if (ModelState.IsValid)
            {
                tmp.Order.User
                        .Materials.Add(tmp.Order.Material);
                db.SaveChanges();
                return RedirectToAction("Index", "MyMaterials");
            }
            ViewBag.UserId = new SelectList(
                db.Users, "Id", "AsString", order.UserId);
            ViewBag.MaterialId = new SelectList(
                db.Materials, "Id", "AsString", order.MaterialId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? userId, int? materialId)
        {
            ViewBag.Name = getName();
            ViewBag.Besteller = getRoleBesteller();
            ViewBag.Systemadm = getRoleSystemadm();
            var tmp = GetOrderOrError(userId, materialId);
            if (tmp.Error != null) return tmp.Error;
            return View(tmp.Order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? userId, int? materialId)
        {
            var tmp = GetOrderOrError(userId, materialId);
            if (tmp.Error != null) return tmp.Error;
            tmp.Order.User
                .Materials.Remove(tmp.Order.Material);
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
