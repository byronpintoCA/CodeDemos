using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Range(1900, 2050)]
        public int Year { get; set; }

        [Required]
        public String Manufacturer { get; set; }

        [Required]
        public String Model { get; set; }
    }

}
