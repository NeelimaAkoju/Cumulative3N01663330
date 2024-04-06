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
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController TeacherController = new TeacherDataController();
            List<Teacher> Teachers = TeacherController.ListTeachers(SearchKey);
            return View(Teachers);
        }

        // GET: Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher selectedTeacher = controller.FindTeacher(id);

            return View(selectedTeacher);
        }

        // GET: Teacher/New
        public ActionResult New()
        {
            return View();
        }

        // POST : Teacher/Create 

        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, string HireDate, decimal salary)
        {
            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFName = TeacherFname;
            NewTeacher.TeacherLName = TeacherLname;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = Convert.ToDateTime(HireDate);
            NewTeacher.Salary = salary;

            TeacherDataController dataController = new TeacherDataController();
            dataController.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }

        //GET : /Author/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController DataController = new TeacherDataController();
            Teacher TeacherDetails  = DataController.FindTeacher(id);
            //Will return to delete confirmation page with techaer details for confirmation
            return View(TeacherDetails);
        }


        //POST : /Author/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherDataController DataController = new TeacherDataController();
            DataController.DeleteTeacher(id);
            ClassDataController classController = new ClassDataController();
            classController.updateClass(id);
            return RedirectToAction("List");
        }



    }
}