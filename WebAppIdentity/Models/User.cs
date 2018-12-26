using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebAppIdentity.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }
    }

    //public enum GenderEnum
    //{
    //    Male = 1,
    //    Female = 2
    //}
}
