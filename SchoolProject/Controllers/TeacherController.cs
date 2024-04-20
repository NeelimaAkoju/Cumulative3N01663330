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
            Boolean success =  dataController.AddTeacher(NewTeacher);
            if (!success)
            {
                //if employee number already exists return back to the add page
                return RedirectToAction("New");
            }
            //else return to list 
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

        
        /// GET /Teacher/Update/1
        public ActionResult Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);

            return View(SelectedTeacher);
        }

        /// <summary>
        /// Updates the information of a specific teacher in the system.
        /// </summary>
        /// <param name="id">The ID of the teacher to update.</param>
        /// <param name="TeacherFname">The updated first name of the teacher.</param>
        /// <param name="TeacherLname">The updated last name of the teacher.</param>
        /// <param name="EmployeeNumber">The updated employee number of the teacher.</param>
        /// <param name="HireDate">The updated hire date of the teacher.</param>
        /// <param name="Salary">The updated salary of the teacher.</param>
        /// <returns>
        /// A response indicating the success or failure of the operation.
        /// Returns a redirect to the details page of the updated teacher if successful.
        /// Returns the "Update Teacher" view with an error message if the provided information is missing or incorrect.
        /// </returns>
        /// <example>
        /// Example of POST request body:
        /// POST /Teacher/Update/{id}
        /// {
        ///     "TeacherFname": "UpdatedFirstName",
        ///     "TeacherLname": "UpdatedLastName",
        ///     "EmployeeNumber": "UpdatedEmployeeNumber",
        ///     "HireDate": "2024-04-15",
        ///     "Salary": 100
        /// }
        /// </example>
        [HttpPost]
        public ActionResult Update(int id, string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, decimal? Salary)
        {
            TeacherDataController controller = new TeacherDataController();

            Teacher Teacher = new Teacher();
            Teacher.TeacherFName = TeacherFname;
            Teacher.TeacherLName = TeacherLname;
            Teacher.EmployeeNumber = EmployeeNumber;
            Teacher.HireDate = HireDate;
            Teacher.Salary = Salary ?? 0;

            controller.UpdateTeacher(id, Teacher);

            return RedirectToAction("Show/" + id);
        }


    }
}
