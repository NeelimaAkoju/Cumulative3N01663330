using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolProject.Models;
using MySql.Data.MySqlClient;

namespace SchoolProject.Controllers
{
    public class StudentDataController : ApiController
    {
        //Initializing varaible for the School database context 
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of student in the system
        /// </summary>
        /// <example>GET api/StudentData/ListStudents</example>
        /// <returns>
        /// A list of Student objects.
        /// </returns>
        [HttpGet]
        [Route("api/StudentData/ListStudents")]
        public IEnumerable<Student> ListStudents()
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM students";
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Student> Students = new List<Student> {};
            while (ResultSet.Read())
            {
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFullName = ResultSet["studentfname"].ToString() + " " + ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                DateTime StudentEnrolDate = (DateTime)ResultSet["enroldate"];
                
                Student NewStudent = new Student();
                NewStudent.StudentId = StudentId;
                NewStudent.StudentFullName = StudentFullName;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.StudentEnrolDate = StudentEnrolDate;

                //Add the Student Name to the List
                Students.Add(NewStudent);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of Student names
            return Students;
        }

        /// <summary>
        /// Returns an individual student from the database by specifying the primary key studentID
        /// </summary>
        /// <example>GET api/StudentData/FindStudent/1</example>
        /// <param name="id">the Student's ID in the database</param>
        /// <returns>An Student object</returns>
        [HttpGet]
        [Route("api/StudentData/FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            Student NewStudent = new Student();

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM students WHERE studentid = " + id.ToString() + ";";
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFullName = ResultSet["studentfname"].ToString() + " " + ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                DateTime StudentEnrolDate = (DateTime)ResultSet["enroldate"];

                NewStudent.StudentId = StudentId;
                NewStudent.StudentFullName = StudentFullName;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.StudentEnrolDate = StudentEnrolDate;
            }
            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            return NewStudent;
        }
    }
}
