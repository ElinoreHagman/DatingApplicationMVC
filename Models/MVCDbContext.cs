using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC_Dating_Project.Models
{
    public class MVCDbContext : DbContext
    {
        public DbSet<Message> Message { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Relation> Relation { get; set; }
        public DbSet<Visit> Visitor { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<FriendRequest> Request { get; set; }

        public MVCDbContext(): base("Dating-Project") { }
    }
}