using System.ComponentModel.DataAnnotations;

namespace PandaWeb.Models.User
{
    public class DeleteAccountModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool IAgree { get; set; }

    }
}
