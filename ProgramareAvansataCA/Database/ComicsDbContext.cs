using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using ProgramareAvansataCA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgramareAvansataCA.Database
{
    public class ComicsDbContext : DbContext
    {
        public DbSet<ComicBook> ComicBooks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ComicBookCollection> Collections { get; set; }
        public DbSet<UserCollectionMapping> UserCollectionMappings { get; set; }

        protected ComicsDbContext()
        {
        }
        public ComicsDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
