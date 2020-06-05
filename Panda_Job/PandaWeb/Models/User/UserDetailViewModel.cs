using System.Collections.Generic;

namespace PandaWeb.Models.User
{
    public class UserDetailViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }        
        public string SecondContactNumber { get; set; }
        public string RegisteredOn { get; set; }

    }
}
