using MySql.Data.MySqlClient;
using SchoolProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SchoolProject.Controllers
{
    public class TeacherDataController : ApiController
    {
        //Initializing varaible for the School database context 
        private SchoolDbContext School = new SchoolDbContext();


        /// <summary>
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <returns>
        /// A list of Teacher objects.
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers")]
        public List<Teacher> ListTeachers()
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Teachers";
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Authors
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();

                Teacher teacher = new Teacher();
                teacher.TeacherId = TeacherId;
                teacher.TeacherFName = TeacherFname;
                teacher.TeacherLName = TeacherLname;
                teacher.EmployeeNumber = EmployeeNumber;

                Teachers.Add(teacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            return Teachers;
        }


        /// <summary>
        /// Returns an individual teacher from the database by specifying the primary key teacherID
        /// </summary>
        /// <example>GET api/TeacherData/FindTeacher/1</example>
        /// <param name="id">the teacher's ID in the database</param>
        /// <returns>An Teacher object</returns>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            Teacher Teacher = new Teacher();
            List<Class> Classes = new List<Class>();

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM teachers JOIN classes ON teachers.teacherid= classes.teacherid WHERE teachers.teacherid = " + id.ToString() + ";";
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string TeacherEmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime TeacherHireDate = (DateTime)ResultSet["hiredate"];
                decimal TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);

                Teacher.TeacherId = TeacherId;
                Teacher.TeacherFName = TeacherFname;
                Teacher.TeacherLName = TeacherLname;
                Teacher.EmployeeNumber = TeacherEmployeeNumber;
                Teacher.HireDate = TeacherHireDate;
                Teacher.Salary = TeacherSalary;

                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassCode = ResultSet["classcode"].ToString();
                string ClassName = ResultSet["classname"].ToString();
                DateTime ClassStartDate = (DateTime)ResultSet["startdate"];
                DateTime ClassFinishDate = (DateTime)ResultSet["finishdate"];

                Class NewClass = new Class();
                NewClass.ClassId = ClassId;
                NewClass.ClassCode = ClassCode;
                NewClass.ClassName = ClassName;
                NewClass.StartDate = ClassStartDate;
                NewClass.FinishDate = ClassFinishDate;
            
                //Add the Class details to the List
                Classes.Add(NewClass);
                Teacher.Classes = Classes;
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();


            return Teacher;
        }

    }
}
