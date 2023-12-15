using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class BorrowController : Controller
    {
        GHOSHEntities db = new GHOSHEntities();

        // GET: Borrow
        public ActionResult Borrow(int id)
        {
            int bookid = id;
            var sname = Convert.ToString(Session["Student_Name"]);
            var studentid = Convert.ToInt32(Session["Student_Id"]);
            var bookname = db.books.Where(a => a.Book_Id == bookid).FirstOrDefault();
          
            BorrowBook b = new BorrowBook();
            b.Book_Id = bookid;
            b.Student_Name = sname;
            b.Transaction_Date = System.DateTime.Now.ToString("yyyy-MM-dd");
            b.Book_Name = bookname.Book_Name;
            b.Student_Id = studentid;



            var check = db.BorrowBook.Where(a => a.Book_Id == bookid && a.Student_Id == studentid).FirstOrDefault();
            if(check == null) // check one student issue book only one 
            {
                db.BorrowBook.Add(b);
                db.SaveChanges();
                var bookavailable = db.books.Where(a => a.Book_Id == bookid).FirstOrDefault();
                bookavailable.Available = bookavailable.Available - 1;
                db.SaveChanges();
                TempData["Message"] = "Book Borrowed Successfully";
                return RedirectToAction("Index", "Book");

            }
            else
            {
                TempData["ErrorMessage"] = "You cannot issue more than one book at a time.";
                return RedirectToAction("Index", "Book");

            }



        }
        public ActionResult View_History()
        {
            
            return View(db.BorrowBook.ToList());
        }
        public ActionResult my_books()
        {
            var studentid = Convert.ToInt32(Session["Student_Id"]);
            var onlyshowbooks = db.BorrowBook.Where(a => a.Student_Id == studentid).ToList();




            //var booksIssuedByStudent = db.Books.Where(b => b.StudentID == studentID).ToList();


            return View(onlyshowbooks);
        }
        public ActionResult Manage_Students()
        {

            return View(db.Student.ToList());
        }
        public ActionResult Remove_Student(int id)
        {
            var removestudent = db.Student.Where(a => a.Student_Id == id).FirstOrDefault();
            db.Student.Remove(removestudent);
            db.SaveChanges();
            TempData["removeMessage"] = "Student Removed Successfully";

            return RedirectToAction("Manage_Students","Borrow");
        }
        public ActionResult Return(int id)
        {
           
            var returnbook = db.BorrowBook.Where(a => a.BorrowBook_Id == id).FirstOrDefault();
            var x = returnbook.Book_Id; // store bookid to x
            db.BorrowBook.Remove(returnbook);
            db.SaveChanges();

            var bookavailable = db.books.Where(b => b.Book_Id == x).FirstOrDefault(); // fetch bookid from x 
            bookavailable.Available = bookavailable.Available + 1;
            db.SaveChanges();
            TempData["Message1"] = "Book Returned Successfully";
            return RedirectToAction("Index", "Book"); 
            
        }
    }
}