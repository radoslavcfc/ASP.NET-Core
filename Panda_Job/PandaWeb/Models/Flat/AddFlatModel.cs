using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PandaWeb.Models.Flat
{
    public class AddFlatModel
    {
        public int? Floor { get; set; }
        public string Entrance { get; set; }

        [Required]
        public int Apartment { get; set; }
    }
}
