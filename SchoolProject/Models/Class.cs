using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolProject.Models
{
    public class Class
    {
        //Fields for Course defined in Database
        public int ClassId;
        public string ClassCode;
        public int TeacherId;
        public string ClassName;
        public DateTime StartDate;
        public DateTime FinishDate;
    }
}