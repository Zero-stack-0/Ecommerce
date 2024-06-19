namespace Entities.Models
{
    public class State
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CountryId { get; set; }
        public ICollection<City> City { get; set; }
    }
}