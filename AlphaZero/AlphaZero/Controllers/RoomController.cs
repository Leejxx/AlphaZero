﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlphaZero.Models;

namespace AlphaZero.Controllers
{
    public class RoomController : Controller
    {
        private db_roomrentalEntities db = new db_roomrentalEntities();

        // GET: Room
        public ActionResult Index()
        {
            var rooms = db.rooms.Include(r => r.floor);
            return View(rooms.ToList());
        }

        // GET: Room/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            room room = db.rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Room/Create
        public ActionResult Create()
        {
            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_id");
            return View();
        }

        // POST: Room/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "room_id,floor_id,room_number,room_price,room_status")] room room)
        {
            if (ModelState.IsValid)
            {
                if (db.rooms.Any(x => x.room_number == room.room_number && x.floor_id == room.floor_id))
                {
                    ViewBag.Message = "Room with the same number and floor already exists";
                    ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_id", room.floor_id);
                    return View(room);
                }

                db.rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_id", room.floor_id);
            return View(room);
        }

        // GET: Room/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            room room = db.rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_id", room.floor_id);
            return View(room);
        }

        // POST: Room/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "room_id,floor_id,room_number,room_price,room_status")] room room)
        {
            if (ModelState.IsValid)
            {
                if (db.rooms.Any(x => (x.room_number == room.room_number && x.floor_id == room.floor_id) && (x.room_id != room.room_id)))
                {
                    ViewBag.Message = "Room with the same number and floor already exists";
                    ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_id", room.floor_id);
                    return View(room);
                }
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_id", room.floor_id);
            return View(room);
        }

        // GET: Room/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            room room = db.rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            room room = db.rooms.Find(id);
            db.rooms.Remove(room);
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