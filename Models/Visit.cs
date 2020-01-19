using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Dating_Project.Models
{
    public class Visit
    {
        [Key]
        public int VisitId { get; set; }

        public string VisitorId { get; set; }
        public string UserId { get; set; }
    }
}