using SchoolProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolProject.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        // GET: Teacher/List
        public ActionResult List()
        {
            TeacherDataController TeacherController = new TeacherDataController();
            List<Teacher> Teachers = TeacherController.ListTeachers();
            return View(Teachers);
        }

        // GET: Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher selectedTeacher = controller.FindTeacher(id);

            return View(selectedTeacher);
        }

    }
}