using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace CoursesPlatform.EntityFramework.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public DateTime RegisteredDate { get; set; }

        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }

    }
}
