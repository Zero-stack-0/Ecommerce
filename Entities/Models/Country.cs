namespace Entities.Models
{
    public class Country
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<State> State { get; set; }
    }
}