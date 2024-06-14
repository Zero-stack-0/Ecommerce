namespace Data.Repository.Interface
{
    public interface IBaseRepository
    {
        public void Add<T>(T item);
        public void AddRange<T>(IEnumerable<T> items);
        public void Update<T>(T item);
        public void UpdateRange<T>(IEnumerable<T> items);
        public void Delete<T>(T item);
        public void DeleteRange<T>(IEnumerable<T> items);
        public void Attach<T>(T item);
        public void AttachRange<T>(IEnumerable<T> items);
        public Task SaveAsync();
    }
}