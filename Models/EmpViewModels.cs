using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class EmpViewModels
    {
        [Required]
        [Display(Name = "First Name")]
        public string EmpFName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string EmpLName { get; set; }
        [Required]
        [Display(Name = "ID")]
        public string EmpID { get; set; }
    }
}