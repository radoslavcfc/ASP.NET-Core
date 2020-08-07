using System.ComponentModel.DataAnnotations;

namespace Agency.Web.Models.Names
{
    public class CreateNamesModel
    {
        [Required]        
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
