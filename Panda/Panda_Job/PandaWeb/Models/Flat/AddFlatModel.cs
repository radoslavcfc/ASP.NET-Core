using System.ComponentModel.DataAnnotations;

namespace PandaWeb.Models.Flat
{
    public class AddFlatModel
    {
        [Range(1, 1000, ErrorMessage = "Only positive number allowed")]
        public int? Floor { get; set; }
        public string Entrance { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Only positive number allowed")]
        public int Apartment { get; set; }
    }
}
