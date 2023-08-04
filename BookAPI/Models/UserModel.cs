using System.ComponentModel.DataAnnotations;

namespace BookAPI.Models
{
    public class UserModel
    {
        [Key]
        public Guid userId { get; set; }

        [Required(ErrorMessage = "Username or Email is required")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "username must be within 4-30 characters")]
        public string emailOrUsername { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(80, MinimumLength = 8, ErrorMessage = "password must be within 8-20 chaacters")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Must contain an Uppercase, Lowercase, Special character and Digit")]
        public string password { get; set; }

    }
}
