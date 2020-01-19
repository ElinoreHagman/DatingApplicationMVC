using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MVC_Dating_Project.Models
{
    public class Profile
    {
        [Key]
        public string ProfileId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please write your first name.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please write your last name.")]
        public string LastName { get; set; }

        [Required]
        [Range(18,99, ErrorMessage = "Your age has to be between 18 and 99.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Please click in your orientation")]
        public string Sexuality { get; set; }

        [Required(ErrorMessage = "Please click in your gender")]
        public string Gender { get; set; }
        
        [DataType(DataType.Upload)]
        [Display(Name = "Upload")]
        [Required(ErrorMessage = "Please choose file to upload.")]

        public string ImagePath { get; set; }

        public bool IsActive { get; set; }

        [XmlIgnore]
        [InverseProperty("TheCategorized")]
        public IList<Category> TheCategorized { get; set; }

        [XmlIgnore]
        [InverseProperty("Categorizer")]
        public IList<Category> Categorizer { get; set; }

        [XmlIgnore]
        [InverseProperty("Author")]
        public IList<Message> WrittenMessages { get; set; }

        [XmlIgnore]
        [InverseProperty("Reciever")]
        public IList<Message> RecievedMessages { get; set; }

        [XmlIgnore]
        [InverseProperty("User")]
        public IList<Relation> Users { get; set; }

        [XmlIgnore]
        [InverseProperty("Friend")]
        public IList<Relation> Friends { get; set; }

        [XmlIgnore]
        [InverseProperty("Getter")]
        public IList<FriendRequest> RequestRecievers { get; set; }

        [XmlIgnore]
        [InverseProperty("Asker")]
        public IList<FriendRequest> RequestSenders { get; set; }
    }
}