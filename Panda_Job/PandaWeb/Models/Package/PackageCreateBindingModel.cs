using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PandaWeb.Models.Package
{
    public class PackageCreateBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Description invalid.", MinimumLength = 5)]
        public string Description { get; set; }

        [Required]
        [Display(Name ="Weight of package in kg")]       
        public double Weight { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Address invalid.", MinimumLength = 5)]
        public string ShippingAddress { get; set; }

        [Required]
        public string Recipient { get; set; }

        public IEnumerable<UserDropDownModel> UsersCollection { get; set; }
    }
}
