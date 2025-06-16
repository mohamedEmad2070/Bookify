namespace Bookify.Web.Core.Models
{
    public class BookCategory
    {
        public int BookId { get; set; }
        public Book? Book { get; set; }

        public int CategoryID { get; set; }
        public Category? Category { get; set; }
    }
}
