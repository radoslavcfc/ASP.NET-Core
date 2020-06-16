using System.ComponentModel.DataAnnotations;

namespace PandaWeb.Models.User
{
    public class HardDeleteUserModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        [Required]
        public bool Confirm { get; set; }
    }
}
