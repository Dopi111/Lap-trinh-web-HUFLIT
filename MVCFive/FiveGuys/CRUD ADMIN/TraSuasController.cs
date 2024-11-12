using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FiveGuys.Models;

namespace FiveGuys.CRUD_ADMIN
{
    public class TraSuasController : Controller
    {
        private FiveGuysProductEntities4 db = new FiveGuysProductEntities4();

        // GET: TraSuas
        public ActionResult Index()
        {

            return View(db.TraSuas.ToList());
        }

        // GET: TraSuas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TraSua traSua = db.TraSuas.Find(id);
            if (traSua == null)
            {
                return HttpNotFound();
            }
            return View(traSua);
        }

        // GET: TraSuas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TraSuas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TenSanPham,MoTa,Loai,NgayThem,GiaTien,HinhAnhText,GiaTienFormatted")] TraSua traSua)
        {
            if (ModelState.IsValid)
            {
                db.TraSuas.Add(traSua);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(traSua);
        }

        // GET: TraSuas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TraSua traSua = db.TraSuas.Find(id);
            if (traSua == null)
            {
                return HttpNotFound();
            }
            return View(traSua);
        }

        // POST: TraSuas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenSanPham,MoTa,Loai,NgayThem,GiaTien,HinhAnhText,GiaTienFormatted")] TraSua traSua)
        {
            if (ModelState.IsValid)
            {
                db.Entry(traSua).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(traSua);
        }

        // GET: TraSuas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TraSua traSua = db.TraSuas.Find(id);
            if (traSua == null)
            {
                return HttpNotFound();
            }
            return View(traSua);
        }

        // POST: TraSuas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TraSua traSua = db.TraSuas.Find(id);
            db.TraSuas.Remove(traSua);
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
