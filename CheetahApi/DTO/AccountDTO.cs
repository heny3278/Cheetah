using System.ComponentModel.DataAnnotations;

namespace CheetahApi.DTO
{
    public class AccountDTO
    {
        public int AccountId { get; set; }

        [Required]
        [MaxLength(128)]
        public string CompanyName { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Website { get; set; }

    }
}
