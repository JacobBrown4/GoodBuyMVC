using GeneralStore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GeneralStore.MVC.Controllers
{
    public class TransactionController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Transaction
        public ActionResult Index()
        {
            return View(_db.Transactions.OrderBy(t => t.TimeOfPurchase).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null) return HttpNotFound();

            return View(transaction);
        }

        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(_db.Customers, "CustomerId", "FullName");
            ViewBag.ProductId = new SelectList(_db.Products, "ProductId", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Transaction transaction)
        {
            transaction.TimeOfPurchase = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (_db.Customers.Find(transaction.CustomerId) == null)
                    return HttpNotFound("Customer not found!");
                var product = _db.Products.Find(transaction.ProductId);
                if (product == null)
                    return HttpNotFound("Product not found!");
                if (transaction.ItemCount > product.InventoryCount)
                    return HttpNotFound("There isn't enough product in inventory for that.");

                _db.Transactions.Add(transaction);
                product.InventoryCount -= transaction.ItemCount;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(_db.Customers, "CustomerId", "FullName");
            ViewBag.ProductId = new SelectList(_db.Products, "ProductId", "Name");
            return View(transaction);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No id entered");

            var transaction = _db.Transactions.Find(id);
            if (transaction == null) return HttpNotFound("No Transaction found by that id");

            return View(transaction);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete (int id)
        {
            Transaction transaction = _db.Transactions.Find(id);
            var product = _db.Products.Find(transaction.ProductId);
            product.InventoryCount += transaction.ItemCount;
            _db.Transactions.Remove(transaction);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit (int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No id entered");
            Transaction transaction = _db.Transactions.Find(id);

            if (transaction == null) return HttpNotFound("No transaction found with that id");
            ViewBag.CustomerId = new SelectList(_db.Customers, "CustomerId", "FullName");
            ViewBag.ProductId = new SelectList(_db.Products, "ProductId", "Name");
            return View(transaction);
        }

        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(transaction).State = System.Data.Entity.EntityState.Modified;
                var oldTransaction = _db.Transactions.Find(transaction.TransactionId);
                var oldProduct = _db.Products.Find(oldTransaction.ProductId);
                oldProduct.InventoryCount += oldTransaction.ItemCount;


                var newProduct = _db.Products.Find(transaction.ProductId);
                if (transaction.ItemCount > newProduct.InventoryCount)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "There is not enough inventory for that change");
                
                newProduct.InventoryCount -= transaction.ItemCount;


                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.CustomerId = new SelectList(_db.Customers, "CustomerId", "FullName");
            ViewBag.ProductId = new SelectList(_db.Products, "ProductId", "Name");
            return View(transaction);
        }
    }
}