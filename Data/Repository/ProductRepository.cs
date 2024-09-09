using Data.Repository.Interface;

namespace Data.Repository
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        private readonly EcommerceDbContext context;
        public ProductRepository(EcommerceDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}