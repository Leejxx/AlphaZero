﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaZero.Controllers
{
    public class CleanerController : Controller
    {
        // GET: Cleaner
        public ActionResult Index()
        {
            return View();
        }

        // GET: Cleaner/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Cleaner/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cleaner/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cleaner/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cleaner/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cleaner/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cleaner/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
