using System.ComponentModel.DataAnnotations;

namespace BookAPI.ViewModel
{
    public class AuthorViewModel
    {

        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be within 3-30 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public int Gender { get; set; }
    }
}
