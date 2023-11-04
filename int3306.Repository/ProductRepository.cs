using int3306.Entities;
using int3306.Repository.Shared;
using Microsoft.EntityFrameworkCore;

namespace int3306.Repository
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(DataContext dataContext) : base(dataContext) {}

        public override async Task<IBaseResult<Product>> Get(int id)
        {
            try
            {
                var r = await DataContext.GetDbSet<Product>()
                    .Include(product => product.ProductType)
                    .FirstOrDefaultAsync(e => e.Id == id);
                return r != null ? BaseResult<Product>.FromSuccess(r) : BaseResult<Product>.FromNotFound();
            }
            catch (Exception e)
            {
                return BaseResult<Product>.FromError(e.ToString());
            }
        }
    }
}