using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProgramareAvansataCA.Database;
using ProgramareAvansataCA.ViewModels;

namespace ProgramareAvansataCA.Controllers
{
    public class CollectionController : BaseSiteController<CollectionViewModel>
    {
        private ComicBookController _comicBookController;

        public CollectionController(ComicsDbContext ctx) : base(ctx)
        {
            _comicBookController = new ComicBookController(ctx);
        }

        public override async Task Add(CollectionViewModel vm)
        {
            if (vm == null)
            {
                throw new ArgumentNullException(nameof(vm));
            }
            if (vm.Name == null)
            {
                throw new ArgumentException("Collection Name is null");
            }

            var collection = new ComicBookCollection()
            {
                Name = vm.Name
            };
            _ctx.Collections.Add(collection);
            await _ctx.SaveChangesAsync();
        }

        public async Task AddToCollection(ComicBookViewModel cbvm, CollectionViewModel cvm)
        {
            if (cbvm == null)
                return;

            if (cvm == null)
                return;

            cbvm.CollectionId = cvm.Id;

            await _comicBookController.Edit(cbvm);
        }

        public async Task RemoveFromCollection(ComicBookViewModel cbvm, CollectionViewModel cvm)
        {
            if (cbvm == null)
                return;

            if (cvm == null)
                return;

            var comicBook = await _ctx.ComicBooks.FirstOrDefaultAsync(x => x.Id == cbvm.Id);
            if (comicBook == null)
                return;

            cbvm.CollectionId = null;
            comicBook.CollectionId = null;

            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> IsInUserCollection(int colId, int userId)
        {
            var q = await _ctx.UserCollectionMappings.FirstOrDefaultAsync(x => x.CollectionId == colId && x.UserId == userId);
            return q != null;

        }

        public override async Task Delete(int id)
        {
            var comicBook = await _ctx.Collections.FirstOrDefaultAsync(x => x.Id == id);
            if (comicBook == null)
                throw new ArgumentException($"A Collction with the given '{id}'was not found");
            _ctx.Collections.Remove(comicBook);
            await _ctx.SaveChangesAsync();
        }

        public override async Task Edit(CollectionViewModel vm)
        {
            var collection = await GetByIdAsync(vm.Id);
            if (collection == null)
                throw new ArgumentException($"A Comic book with the given '{vm.Id}' was not found");

            if (vm.Name != null)
                collection.Name   = vm.Name;
        }

        public override async Task<List<CollectionViewModel>> GetAsync()
        {
            var collections = await (from collection in _ctx.Collections
                                     select new CollectionViewModel()
                                     {
                                         Id = collection.Id,
                                         Name = collection.Name
                                     }).ToListAsync();

            foreach (var col in collections)
            {
                col.ComicBooks = await (from x in _ctx.ComicBooks
                                        where x.CollectionId == col.Id
                                        select new ComicBookViewModel()
                                        {
                                            Author = x.Author,
                                            Id = x.Id,
                                            CollectionId = x.CollectionId,
                                            Description = x.Description,
                                            ImageUrl = x.ImageUrl,
                                            IssueDate = x.IssueDate,
                                            Title = x.Title
                                        }).ToListAsync();
            }

            return collections;
        }

        public override async Task<CollectionViewModel> GetByIdAsync(int id)
        {
            var collection = await (from c in _ctx.Collections
                                    where c.Id == id
                                    select new CollectionViewModel()
                                    {
                                        Id = c.Id,
                                        Name = c.Name
                                    }).FirstOrDefaultAsync();

            collection.ComicBooks = await (from x in _ctx.ComicBooks
                                           where x.CollectionId == collection.Id
                                           select new ComicBookViewModel()
                                           {
                                               Author = x.Author,
                                               Id = x.Id,
                                               CollectionId = x.CollectionId,
                                               Description = x.Description,
                                               ImageUrl = x.ImageUrl,
                                               IssueDate = x.IssueDate,
                                               Title = x.Title
                                           }).ToListAsync();

            return collection;
        }
    }

}