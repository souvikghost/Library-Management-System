using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Models;


namespace Library.Controllers
{
    public class LibrarianController : Controller
    {
        GHOSHEntities db = new GHOSHEntities();
        //GET: Librarian



        public ActionResult Index()
        {
            return View(db.books.ToList());

        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        public ActionResult Create(books b, HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    string ImageName = Path.GetFileName(file.FileName);
                    string physicalPath = Server.MapPath("~/Images/" + ImageName);
                    file.SaveAs(physicalPath);

                }

                b.Available = b.Stock;
                // ---- Edited Here -------
                b.Book_Image = file.FileName;
                db.books.Add(b);
                db.SaveChanges();
                ModelState.Clear();
                ViewBag.SuccessMessage = "Book added Successfully";

                return RedirectToAction("Index","Librarian");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            var detail = db.books.Where(a => a.Book_Id == id).FirstOrDefault();
            return View(detail);
        }

        // POST: Book/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, books b)
        {
            try
            {
                // TODO: Add update logic here
                db.Entry(b).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            var detail = db.books.Where(a => a.Book_Id == id).FirstOrDefault();
            return View(detail);
        }

        // POST: Book/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, books b)
        {
            try
            {
                // TODO: Add delete logic here
                var detail = db.books.Where(a => a.Book_Id == id).FirstOrDefault();
                db.books.Remove(detail);
                db.SaveChanges();
                ViewBag.SuccessMessage = "Book Deleted Successfully";
                return View();
            }
            catch
            {
                return View();
            }
        }















        //[HttpGet]
        //public ActionResult Login()
        //{

        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Login(Librarian l)
        //{
        //    var checklogin = db.Librarian.Where(a => a.User_Name == l.User_Name && a.Password == l.Password).FirstOrDefault();
        //    if (checklogin != null)
        //    {
        //        //Session["Student_Id"] = s.Student_Id.ToString();

        //        Session["User_Name"] = l.User_Name.ToString();

        //        ViewBag.mesg = "Successful";
        //        return RedirectToAction("Index", "Book");

        //    }
        //    else
        //    {
        //        ViewBag.mesg = "Invalis Username & Password";
        //    }
        //    return View();
        //}
    }
}