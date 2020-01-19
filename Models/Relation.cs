using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Dating_Project.Models
{
    public class Relation
    {
        [Key]
        public int RelationId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public Profile User { get; set; }

        [ForeignKey("Friend")]
        public string FriendId { get; set; }
        public Profile Friend { get; set; }

    }
}