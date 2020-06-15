using Microsoft.AspNetCore.Mvc.ModelBinding;
using PandaWeb.Models.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PandaWeb.Models.Package
{
    public class PackageCreateModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Description invalid.", MinimumLength = 5)]
        public string Description { get; set; }

        [Required]
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
