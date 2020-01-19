using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Dating_Project.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public string MessageText { get; set; }
        public DateTime Date { get; set; }


        [ForeignKey ("Author")]
        public string AuthorId { get; set; }
        public Profile Author { get; set; }

        [ForeignKey("Reciever")]
        public string RecieverId { get; set; }
        public Profile Reciever { get; set; }
    }

}
