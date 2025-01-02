using System;
using System.ComponentModel.DataAnnotations;

namespace Embarkasi.Models
{
    public class UpdateSektorByLoaderModel
    {
        [Required]
        [StringLength(255)]
        public string Sektor { get; set; }

        [Required]
        [StringLength(255)]
        public string Loader { get; set; }

        [Required]
        [StringLength(255)] 
        public string Transportasi { get; set; }
    }
}
