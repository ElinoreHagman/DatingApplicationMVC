using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Dating_Project.Models
{
    public class ChosenProfileView
    {
        public Profile Profile { get; set; }
        public Relation Relation { get; set; }
        public FriendRequest Requests { get; set; }
        public FriendRequest SentRequest { get; set; }
        public List<Message> Messages { get; set; }

        public List<Message> personalMessages { get; set; }

    }
}