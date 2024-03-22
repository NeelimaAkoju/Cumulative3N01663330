using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolProject.Models
{
    public class Teacher
    {
        //Fields for Teacher defined in Database
        public int TeacherId;
        public string TeacherFName;
        public string TeacherLName;
        public string EmployeeNumber;
        public DateTime HireDate;
        public decimal Salary;
        //To store the classes the Teacher is assigned with
        public List<Class> Classes;
    }
}