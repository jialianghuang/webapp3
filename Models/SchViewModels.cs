using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class SchViewModels
    {
        [Required]
        [Display(Name = "ID")]
        public string EmpID { get; set; }
        [Required]
        [Display(Name = "Date")]
        public string SchDate { get; set; }
        [Required]
        [Display(Name = "Start")]
        public string SchStart { get; set; }
        [Required]
        [Display(Name = "End")]
        public string SchEnd { get; set; }

    }
}