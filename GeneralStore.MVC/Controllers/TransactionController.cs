using GeneralStore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace GeneralStore.MVC.Controllers
{
    public class TransactionController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Transaction
        public ActionResult Index()
        {
            List<Transaction> transactionList = _db.Transactions.ToList();
            List<Transaction> orderedTransactions = transactionList.OrderBy(tran => tran.TimeOfTransaction).ToList();
            return View(orderedTransactions);
        }

        public ActionResult Create()
        {
            var customers = new SelectList(_db.Customers.ToList(), "CustomerId", "FullName");
            ViewBag.Customers = customers;
            var products = new SelectList(_db.Products.ToList(), "ProductId", "Name");
            ViewBag.Products = products;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                Customer customer = _db.Customers.Find(transaction.CustomerId);
                if (customer == null) return HttpNotFound("Customer not found");
                Product product = _db.Products.Find(transaction.ProductId);
                if (product == null) return HttpNotFound("Product not found");
                if (transaction.ItemCount > product.InventoryCount) return HttpNotFound("There isn't enough product for this purchase");

                _db.Transactions.Add(transaction);
                product.InventoryCount -= transaction.ItemCount;
                _db.SaveChanges();
                return RedirectToAction("Index");


            }
            return View(transaction);
        }

        // GET: Delete
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null) return HttpNotFound();
            return View(transaction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Transaction transaction = _db.Transactions.Find(id);
            Product product = _db.Products.Find(transaction.ProductId);
            product.InventoryCount += transaction.ItemCount;
            _db.Transactions.Remove(transaction);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Get: Edit
        public ActionResult Edit (int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null) return HttpNotFound();
            var customers = new SelectList(_db.Customers.ToList(), "CustomerId", "FullName");
            ViewBag.Customers = customers;
            var products = new SelectList(_db.Products.ToList(), "ProductId", "Name");
            ViewBag.Products = products;
            return View(transaction);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var oldTransaction = _db.Transactions.AsNoTracking().Where(P => P.TransactionId == transaction.TransactionId).FirstOrDefault();
                //int oldTransaction = _db.Transactions.Find(transaction.TransactionId).ItemCount;
                Product product = _db.Products.Find(transaction.ProductId);
                product.InventoryCount += oldTransaction.ItemCount;
                _db.Entry(transaction).State = System.Data.Entity.EntityState.Modified;
                product.InventoryCount -= transaction.ItemCount;
                if (product.InventoryCount < 0) return HttpNotFound("There isn't enough product for this purchase");
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transaction);
        }
        // GET: Details
        // Transaction/Details/{id}

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Transaction transaction = _db.Transactions.Find(id);

            if (transaction == null) return HttpNotFound();

            return View(transaction);
        }
    }
}