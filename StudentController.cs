using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Models;
namespace Library.Controllers
{
    public class StudentController : Controller
    {
        GHOSHEntities db = new GHOSHEntities();
        // GET: Student
        public ActionResult Index()
        {
            return View(db.Student.ToList());
            //return RedirectToAction("Index", "Book");
        }
        [HttpGet]
        public ActionResult Signup()
        {

            var listcourse = new List<string> { "B.Tech", "M.Tech", "BCA", "MCA" };
            var listsemester = new List<string> { "1", "2", "3", "4","5","6","7","8"};
            ViewBag.list = listcourse;
            ViewBag.list1 = listsemester;
            return View();
        }
        [HttpPost]
        public ActionResult Signup(string email, Student s)
        {
           
            var checkemail = db.Student.Where(a => a.Email_Id == email).FirstOrDefault();
            if (checkemail != null)
            {
                ViewBag.msg = "This Email already exist";
                return View();

            }
            else
            {
                db.Student.Add(s);
                db.SaveChanges();
                Session["Student_Id"] = s.Student_Id.ToString();
                Session["User_Name"] = s.User_Name.ToString();
                return RedirectToAction("Index", "Book");
            }

            
        }
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(Student s)
        
        {
            var checklogin_librarian = db.Librarian.Where(a => a.User_Name == s.User_Name && a.Password == s.Password).FirstOrDefault();
            if (checklogin_librarian != null)
            {
                //Session["Student_Id"] = s.Student_Id.ToString();

                Session["User_Name"] = s.User_Name.ToString();

                ViewBag.mesg = "Successful";
                return RedirectToAction("Index", "Librarian");

            }
            else
            {
                var checklogin_student = db.Student.Where(a => a.User_Name == s.User_Name && a.Password == s.Password).FirstOrDefault();
                if (checklogin_student != null)
                {
                    Session["Student_Id"] = checklogin_student.Student_Id.ToString();
                    Session["Email_Id"] = checklogin_student.Email_Id.ToString();
                    Session["Student_Name"] = checklogin_student.Student_Name.ToString();
                    Session["User_Name"] = s.User_Name.ToString();

                    ViewBag.mesg = "Successful";
                    return RedirectToAction("Index", "Book");

                }
                else
                {
                    ViewBag.mesg = "Invalis Username & Password";
                }
                //ViewBag.mesg = "Invalis Username & Password";
            }
            
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Student");
        }


    }
}