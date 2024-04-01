using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
//import org.hibernate.validator.constraints.UniqueConstraint; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cheetah;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CheetahDB
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [ForeignKey("Account")]
        public int? AccountId { get; set; }

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
