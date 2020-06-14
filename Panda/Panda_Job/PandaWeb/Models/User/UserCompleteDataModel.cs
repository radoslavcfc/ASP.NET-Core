using System.ComponentModel.DataAnnotations;
namespace PandaWeb.Models.User
{
    public class UserCompleteDataModel
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20,MinimumLength =2)]
        public string LastName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Phone]
        public string SecondContactNumber { get; set; }
    }
}
