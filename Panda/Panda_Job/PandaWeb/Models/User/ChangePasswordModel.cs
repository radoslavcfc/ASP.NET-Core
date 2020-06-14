using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PandaWeb.Models.User
{
    public class ChangePasswordModel
    {
        [DisplayName("Your current password")]
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DisplayName("Your new password")]
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DisplayName("Confirm new password")]
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
