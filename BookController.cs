using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Models;
using System.IO;

namespace Library.Controllers
{
    
    public class BookController : Controller
    {
        GHOSHEntities db = new GHOSHEntities();
        // GET: Book
        public ActionResult Index(string searchBy, string search )
        {
            if (searchBy == "Author_Name")
            {
                var books = db.books.Where(a => a.Author_Name == search || search == null).ToList()/*.Where(a=>a.Available!=0).ToList()*/;
                return View(books);
            }
               

            else
                return View(db.books.Where(a => a.Book_Name.StartsWith(search) || search == null).ToList()/*.Where(a => a.Available != 0).ToList()*/);

            //string today = (System.DateTime.Now).ToString("dd-MM-yyyy");
           
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var detail = db.books.Where(a => a.Book_Id == id).FirstOrDefault();
            return View(detail);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        public ActionResult Create(books b , HttpPostedFileBase file)
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

        // GET: Book/Edit/5
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
                //return View();
                return RedirectToAction("Index", "Book");
            }
            catch
            {
                return View();
            }
        }
        //public ActionResult Borrow()
        //{
        //    //var a = Session["Student_Id"].ToString();
        //    //var borrowbook = from test in db.books
        //    //                 where test.Book_Id == id
        //    //                 select test;
        //    return View();
        //    //return RedirectToAction("Update_book", "Borrow");

        //}
        //[HttpPost]
        //public ActionResult Borrow()
        //{
        //    return View();

        //}
    }
}
