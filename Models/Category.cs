using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Dating_Project.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        [ForeignKey("Categorizer")]
        public string CategorizerId { get; set; }
        public Profile Categorizer { get; set; }

        [ForeignKey("TheCategorized")]
        public string TheCategorizedId { get; set; }
        public Profile TheCategorized { get; set; }
    }
}