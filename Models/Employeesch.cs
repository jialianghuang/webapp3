using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Employeesch
    {
        public Employeesch(string emp,string nam)
        {
            this.EmpID = emp;
            this.EmpName = nam;
        }
      
        public string EmpName { get; set; }
        public string EmpID { get; set; }
        public string Date { get; set; }
        public string monstart { get; set; }
        public string monend { get; set; }
        public string tuestart { get; set; }
        public string tueend { get; set; }
        public string wedstart { get; set; }
        public string wedend { get; set; }
        public string thustart { get; set; }
        public string thuend { get; set; }
        public string fristart { get; set; }
        public string friend { get; set; }
        public string satstart { get; set; }
        public string satend { get; set; }


    }
}