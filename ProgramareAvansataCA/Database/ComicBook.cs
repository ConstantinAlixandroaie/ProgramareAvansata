using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgramareAvansataCA.Database
{
    public class ComicBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public DateTimeOffset IssueDate { get; set; }

        public int? CollectionId { get; set; }
        public ComicBookCollection Collection { get; set; }
    }
}
