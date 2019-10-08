using System.Collections.Generic;

namespace ProgramareAvansataCA.ViewModels
{
    public class CollectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ComicBookViewModel> ComicBooks { get; set; }
    }
}
