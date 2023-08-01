using System.ComponentModel.DataAnnotations;

namespace BookAPI.Models
{
    public class AuthorModel
    {
        public Guid Id
        {
            get;
            set;
        }

        public string Name { get; set; }
       
        public int Gender { get; set; }

        public virtual ICollection<BookModel>? Books { get; set; }
    }
}
