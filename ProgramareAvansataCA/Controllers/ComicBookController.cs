using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgramareAvansataCA.Database;
using ProgramareAvansataCA.ViewModels;

namespace ProgramareAvansataCA.Controllers
{
    public class ComicBookController : BaseSiteController<ComicBookViewModel>
    {
        public ComicBookController(ComicsDbContext ctx) : base(ctx)
        {
        }

        /// <summary>
        /// Get a list of all the comic books in the database.
        /// </summary>
        /// <returns></returns>
        public override async Task<List<ComicBookViewModel>> GetAsync()
        {
            var rv = new List<ComicBookViewModel>();
            var comicbooks = await _ctx.ComicBooks.ToListAsync();
            foreach (var comicBook in comicbooks)
            {
                var vm = new ComicBookViewModel()
                {
                    Id = comicBook.Id,
                    Title = comicBook.Title,
                    Author = comicBook.Author,
                    ImageUrl = comicBook.ImageUrl,
                    Description = comicBook.Description,
                    IssueDate = comicBook.IssueDate,

                };
                rv.Add(vm);
            }
            return rv;
        }
        public override async Task<ComicBookViewModel> GetByIdAsync(int id)
        {
            var comicBook = await _ctx.ComicBooks.FirstOrDefaultAsync(x => x.Id == id);
            if (comicBook == null)
            {
                return null;
            }
            var rv = new ComicBookViewModel()
            {
                Id = comicBook.Id,
                Title = comicBook.Title,
                Author = comicBook.Author,
                ImageUrl = comicBook.ImageUrl,
                Description = comicBook.Description,
                IssueDate = comicBook.IssueDate,
            };
            return rv;
        }
        public override async Task Add(ComicBookViewModel vm)
        {
            if (vm == null)
            {
                throw new ArgumentNullException(nameof(vm));
            }
            if (vm.Title == null)
            {
                throw new ArgumentException("Title is null");
            }
            if (vm.Author == null)
            {
                throw new ArgumentException("Author is null");
            }
            if (vm.IssueDate == null)
            {
                throw new ArgumentException("Issue Date is null");
            }

            var comicBook = new ComicBook()
            {
                Author = vm.Author,
                Title = vm.Title,
                ImageUrl = vm.ImageUrl,
                Description = vm.Description,
                IssueDate = vm.IssueDate ?? DateTimeOffset.Now,
            };
            _ctx.ComicBooks.Add(comicBook);
            await _ctx.SaveChangesAsync();
        }
        public override async Task Edit(ComicBookViewModel vm)
        {
            var comicBook = await _ctx.ComicBooks.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (comicBook == null)
                throw new ArgumentException($"A comic book with the given '{vm.Id}' was not found");

            if (vm.CollectionId != null)
                comicBook.CollectionId = vm.CollectionId;

            if (!string.IsNullOrEmpty(vm.Title))
                comicBook.Title = vm.Title;

            if (!string.IsNullOrEmpty(vm.Author))
                comicBook.Author = vm.Author;

            if (!string.IsNullOrEmpty(vm.Description))
                comicBook.Description = vm.Description;

            if (!string.IsNullOrEmpty(vm.ImageUrl))
                comicBook.ImageUrl = vm.ImageUrl;

            //if (vm.IssueDate != null)
            //    comicBook.IssueDate = vm.IssueDate ?? DateTimeOffset.Now;

            _ctx.Attach(comicBook).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();

        }
        public override async Task Delete(int id)
        {
            var comicBook = await _ctx.ComicBooks.FirstOrDefaultAsync(x => x.Id == id);
            if (comicBook == null)
                throw new ArgumentException($"A Comic Book with the given '{id}'was not found");
            _ctx.ComicBooks.Remove(comicBook);
            await _ctx.SaveChangesAsync();
        }
    }
}