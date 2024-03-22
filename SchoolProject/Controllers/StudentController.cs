using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;

namespace SchoolProject.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        // GET: Student/List
        public ActionResult List()
        {
            StudentDataController controller = new StudentDataController();
            IEnumerable<Student> Students = controller.ListStudents();
            return View(Students);
        }

        // GET: Student/Show/{id}
        public ActionResult Show(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student selectedStudent = controller.FindStudent(id);
            return View(selectedStudent);
        }
    }
}