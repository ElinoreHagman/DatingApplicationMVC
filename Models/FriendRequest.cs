using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Dating_Project.Models
{
    public class FriendRequest
    {
        [Key]
        public int RequestId { get; set; }

        [ForeignKey("Getter")]
        public string GetterId { get; set; }
        public Profile Getter { get; set; }

        [ForeignKey("Asker")]
        public string AskerId { get; set; }
        public Profile Asker { get; set; }
    }
}