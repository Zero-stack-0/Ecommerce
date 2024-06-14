using Data.Repository.Interface;

namespace Data.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private readonly EcommerceDbContext context;

        public BaseRepository(EcommerceDbContext context)
        {
            this.context = context;
        }

        public void Add<T>(T item)
        {
            context.Add(item);
        }

        public void AddRange<T>(IEnumerable<T> items)
        {
            context.AddRange(items);
        }

        public void Delete<T>(T item)
        {
            context.Remove(item);
        }

        public void DeleteRange<T>(IEnumerable<T> items)
        {
            context.RemoveRange(items);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Update<T>(T item)
        {
            context.Update(item);
        }

        public void UpdateRange<T>(IEnumerable<T> items)
        {
            context.UpdateRange(items);
        }

        public void Attach<T>(T item)
        {
            context.Attach(item);
        }

        public void AttachRange<T>(IEnumerable<T> items)
        {
            context.AttachRange(items);
        }
    }
}