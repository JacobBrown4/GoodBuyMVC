﻿using GeneralStore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GeneralStore.MVC.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Customer
        public ActionResult Index()
        {
            List<Customer> customerList = _db.Customers.ToList();
            List<Customer> orderedList = customerList.OrderBy(prod => prod.LastName).ThenByDescending(cust => cust.FirstName).ToList();
            return View(orderedList);
        }

        // GET: Customer
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Delete
        // Customer/Delete/{id}

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            Customer customer = _db.Customers.Find(id);
            if (customer == null) return HttpNotFound();
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Customer customer = _db.Customers.Find(id);
            _db.Customers.Remove(customer);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Edit
        // Customer/Edit/{id}
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            Customer customer = _db.Customers.Find(id);
            if (customer == null) return HttpNotFound();
            return View(customer);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Details
        // Customer/Details/{id}

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Customer customer = _db.Customers.Find(id);
            if (customer == null) return HttpNotFound();


            CustomerDetail details = new CustomerDetail()
            {
                FullName = customer.FullName,
                CustomerId = customer.CustomerId,
                Transactions = customer.Transactions.ToList()
            };
            return View(details);
        }
    }
}