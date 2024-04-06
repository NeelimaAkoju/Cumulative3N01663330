using MySql.Data.MySqlClient;
using SchoolProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using static Mysqlx.Expect.Open.Types.Condition.Types;

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
        [Route("api/TeacherData/ListTeachers/{SearchKey}")]
        public List<Teacher> ListTeachers(string SearchKey=null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();
            
            //SQL QUERY
            cmd.CommandText = "Select * from Teachers where lower(teacherlname) like lower(@key) or lower(employeenumber) like lower(@key) or lower(teacherfname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
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

            //Return the final list of Teacher names
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
            cmd.CommandText = "SELECT * FROM teachers LEFT JOIN classes ON teachers.teacherid= classes.teacherid WHERE teachers.teacherid = " + id.ToString() + ";";
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
                int ClassId = 0;
                if (!ResultSet["classid"].Equals(null))
                {
                   ClassId = Convert.ToInt32(ResultSet["classid"]);
                }
                
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


        /// <summary>
        /// Adds a Teacher to the MySQL Database. 
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the Tecahers's table. </param>
        /// <example>
        /// POST api/TecaherData/AddTeacher 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFName":"Neelima",
        ///	"TeacherLName":"Akoju",
        ///	"EmployeeNumber":"T404",
        ///	"HireDate":"28-04-2023"
        ///	"Salary":"20.0"
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody]Teacher NewTeacher)
        {
            if (NewTeacher.EmployeeNumber != null)
            {
                List<Teacher> teacher = ListTeachers(NewTeacher.EmployeeNumber);
                if (teacher.Count > 0)
                {
                  
                }
                else
                {
                    //Create an instance of a connection
                    MySqlConnection Conn = School.AccessDatabase();

                    //Open the connection between the web server and database
                    Conn.Open();

                    //Establish a new command (query) for our database
                    MySqlCommand cmd = Conn.CreateCommand();
                    
                    //Query to add teacher into DB
                    cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@teacherfname, @teacherlname, @employeenumber, @hiredate, @salary)";
                    cmd.Parameters.AddWithValue("@teacherfname", NewTeacher.TeacherFName);
                    cmd.Parameters.AddWithValue("@teacherlname", NewTeacher.TeacherLName);
                    cmd.Parameters.AddWithValue("@employeenumber", NewTeacher.EmployeeNumber);
                    cmd.Parameters.AddWithValue("@hiredate", NewTeacher.HireDate);
                    cmd.Parameters.AddWithValue("@salary", NewTeacher.Salary);

                    cmd.Prepare();

                    cmd.ExecuteNonQuery();

                    Conn.Close();
                }
            }
            
        }

        /// <summary>
        /// Deletes a Teacher from the connected MySQL Database if the ID of that teacher exists. 
        /// </summary>
        /// <param name="id">The ID of the author.</param>
        /// <example>POST /api/TeacherData/DeleteTeacher/3</example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void DeleteTeacher(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }

    }
}
