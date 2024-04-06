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
    public class ClassDataController : ApiController
    {
        //Initializing varaible for the School database context 
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of Classes in the system
        /// </summary>
        /// <example>GET api/ClassData/ListClasses</example>
        /// <returns>
        /// A list of Class objects.
        /// </returns>
        [HttpGet]
        [Route("api/ClassData/ListClasses")]
        public IEnumerable<Class> ListClasses()
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM classes";
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Class> Classes = new List<Class> { };
            while (ResultSet.Read())
            {
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassCode = ResultSet["classcode"].ToString();
                string ClassName = ResultSet["classname"].ToString();
                DateTime ClassStartDate = (DateTime)ResultSet["startdate"];
                DateTime ClassFinishDate = (DateTime)ResultSet["finishdate"];
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);

                Class NewClass = new Class();
                NewClass.ClassId = ClassId;
                NewClass.ClassCode = ClassCode;
                NewClass.ClassName = ClassName;
                NewClass.StartDate = ClassStartDate;
                NewClass.FinishDate = ClassFinishDate;
                NewClass.TeacherId = TeacherId;

                //Add the Class to the List
                Classes.Add(NewClass);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();



            //Return the final list of Class names
            return Classes;
        }

        /// <summary>
        /// Returns an individual Class from the database by specifying the primary key classID
        /// </summary>
        /// <example>GET api/ClassData/FindClass/1</example>
        /// <param name="id">the class's ID in the database</param>
        /// <returns>An Class object</returns>
        [HttpGet]
        [Route("api/ClassData/FindClass/{id}")]
        public Class FindClass(int id)
        {
            Class NewClass = new Class();

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM classes WHERE classid = " + id.ToString() + ";";
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassCode = ResultSet["classcode"].ToString();
                string ClassName = ResultSet["classname"].ToString();
                DateTime ClassStartDate = (DateTime)ResultSet["startdate"];
                DateTime ClassFinishDate = (DateTime)ResultSet["finishdate"];
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);

                NewClass.ClassId = ClassId;
                NewClass.ClassCode = ClassCode;
                NewClass.ClassName = ClassName;
                NewClass.StartDate = ClassStartDate;
                NewClass.FinishDate = ClassFinishDate;
                NewClass.TeacherId = TeacherId;
            }
            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            return NewClass;
        }

        public void updateClass(int teacherId)
        {

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "UPDATE classes set teacherid = null WHERE teacherid = @id";
            cmd.Parameters.AddWithValue("@id", teacherId);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            Conn.Close();

        }

    }
}
