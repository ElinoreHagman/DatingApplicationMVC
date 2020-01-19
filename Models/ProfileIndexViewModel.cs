using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Dating_Project.Models
{
    public class ProfileIndexViewModel
    {
        public List<Profile> Profiles { get; set; }
        public List<FriendRequest> Requests { get; set; }

    }
}