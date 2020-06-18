using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using PandaWeb.Models.User;

namespace PandaWeb.Models.Package
{
    public class PackageCreateModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Description invalid.", MinimumLength = 5)]
        public string Description { get; set; }

        [Required]
        [Range(0.1, 1000, ErrorMessage = "Only positive number allowed")]
        [Display(Name ="Weight in kg")]       
        public double Weight { get; set; }

        [Required]        
        public string ShippingAddress { get; set; }

        [Required]
        public string Recipient { get; set; }
        
        [BindNever]
        public IEnumerable<UserDropDownModel> UsersCollection { get; set; }       
    }
}
