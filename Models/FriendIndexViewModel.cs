using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Dating_Project.Models
{
    public class FriendIndexViewModel
    {
        public List<Relation> Relations { get; set; }
        public List<Profile> Profiles { get; set; }
        public List<Category> Categories { get; set; }
        public string Category { get; set; }
    }
}