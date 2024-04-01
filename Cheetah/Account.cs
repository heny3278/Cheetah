using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace Cheetah
{
    public class Account
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(128)]
        public string CompanyName { get; set; }

        [DefaultValue("n/a")]
        public string Website { get; set; } 

     

       

    }
}