using int3306.Entities;
using int3306.Repository.Shared;

namespace int3306.Repository 
{
    public class ProductThumbnailRepository : BaseRepository<ProductThumbnail> 
    {
        public ProductThumbnailRepository(DataContext dataContext) : base(dataContext) {}
        
    }
    
}