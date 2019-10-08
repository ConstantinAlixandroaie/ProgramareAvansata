using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProgramareAvansataCA.Database;
using ProgramareAvansataCA.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace ProgramareAvansataCA.Controllers
{
    public class UserController : BaseSiteController<UserViewModel>
    {
        private CollectionController _collectionController;

        public UserController(ComicsDbContext ctx) : base(ctx)
        {
            _collectionController = new CollectionController(ctx);
        }

        public override Task Add(UserViewModel vm)
        {
            throw new NotImplementedException();
        }

        public async Task AddCollectionToUser(int colId, UserViewModel uvm)
        {
            var col = await _ctx.Collections.FirstOrDefaultAsync(x => x.Id == colId);
            if (col == null)
                return;

            var usr = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == uvm.Id);
            if (usr == null)
                return;

            //exista deja maparea asta?
            var curMap = await _ctx.UserCollectionMappings.FirstOrDefaultAsync(x => x.CollectionId == col.Id && x.UserId == usr.Id);
            if (curMap != null)
                return;

            var map = new UserCollectionMapping()
            {
                CollectionId = col.Id,
                UserId = usr.Id
            };

            _ctx.UserCollectionMappings.Add(map);
            await _ctx.SaveChangesAsync();
        }

        public async Task RemoveCollectionFromUser(int colId, UserViewModel uvm)
        {
            var col = await _ctx.Collections.FirstOrDefaultAsync(x => x.Id == colId);
            if (col == null)
                return;

            var usr = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == uvm.Id);
            if (usr == null)
                return;

            //exista deja maparea asta?
            var curMap = await _ctx.UserCollectionMappings.FirstOrDefaultAsync(x => x.CollectionId == col.Id && x.UserId == usr.Id);
            if (curMap == null)
                return;

            _ctx.UserCollectionMappings.Remove(curMap);
            await _ctx.SaveChangesAsync();
        }

        public async Task Register(string email, string password)
        {
            var hashAlgorithm = new SHA1CryptoServiceProvider();

            var u = new User()
            {
                Username = email,
                Password = Encoding.Default.GetString(hashAlgorithm.ComputeHash(Encoding.Default.GetBytes(password)))
            };

            _ctx.Users.Add(u);
            await _ctx.SaveChangesAsync();
        }

        public override Task Delete(int id)
        {
            return null; // will not be used
        }

        public override Task Edit(UserViewModel vm)
        {
            throw new NotImplementedException();
        }

        public override Task<List<UserViewModel>> GetAsync()
        {
            return null; // will not be used
        }

        public override async Task<UserViewModel> GetByIdAsync(int id)
        {
            var usr = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (usr == null)
                return null;

            var collections = await (from mapping in _ctx.UserCollectionMappings
                                     join collection in _ctx.Collections on mapping.CollectionId equals collection.Id
                                     where mapping.UserId == usr.Id
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

            return new UserViewModel()
            {
                Collections = collections,
                Id = usr.Id,
                Username = usr.Username
            };
        }
    }

}