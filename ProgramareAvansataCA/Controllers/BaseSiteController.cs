using System.Collections.Generic;
using System.Threading.Tasks;
using ProgramareAvansataCA.Database;

namespace ProgramareAvansataCA.Controllers
{
    public abstract class BaseSiteController<T>
    {
        protected readonly ComicsDbContext _ctx;

        public BaseSiteController(ComicsDbContext ctx)
        {
            _ctx = ctx;
        }

        public abstract Task<List<T>> GetAsync();
        public abstract Task<T> GetByIdAsync(int id);
        public abstract Task Add(T vm);
        public abstract Task Edit(T vm);
        public abstract Task Delete(int id);
    }
}