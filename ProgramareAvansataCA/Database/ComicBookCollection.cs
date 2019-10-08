using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgramareAvansataCA.Database
{
    public class ComicBookCollection
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ComicBook> ComicBooks { get; set; }
    }
}
