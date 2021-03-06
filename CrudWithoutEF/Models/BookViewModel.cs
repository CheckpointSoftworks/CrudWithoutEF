using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudWithoutEF.Models
{
    public class BookViewModel
    {
        [Key]
        public int BookID { get; set; }

        [Required]
        public string Title { get; set;}

        [Required]
        public string Author { get; set; }

        [Required]
        [Range(1,int.MaxValue,ErrorMessage = "Price was not included or was not greater than or equal to 1")]
        public int Price { get; set; }
    }
}
