﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cheetah;

namespace CheetahApi.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }


        [ForeignKey("Account")]
        public int AccountId { get; set; }



        [Required]
        [MaxLength(128)]
        public string FirstName { get; set; }

        [MaxLength(128)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
