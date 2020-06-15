using System.ComponentModel.DataAnnotations;

namespace PandaWeb.Models.Flat
{
    public class AddFlatModel
    {
        public int? Floor { get; set; }
        public string Entrance { get; set; }

        [Required]
        public int? Apartment { get; set; }
    }
}
