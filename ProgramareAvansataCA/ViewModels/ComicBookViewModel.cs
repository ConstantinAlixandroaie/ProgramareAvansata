using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProgramareAvansataCA.ViewModels
{
    public class ComicBookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? IssueDate { get; set; }
        public int? CollectionId { get; internal set; }
    }
}
