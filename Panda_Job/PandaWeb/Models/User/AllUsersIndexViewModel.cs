using System.Collections.Generic;

namespace PandaWeb.Models.User
{
    public class AllUsersIndexViewModel
    {
        public AllUsersIndexViewModel()
        {
            this.AllUsersCollection = new List<UserIndexViewModel>();
        }
        public IList<UserIndexViewModel> AllUsersCollection { get; set; }
    }
}
