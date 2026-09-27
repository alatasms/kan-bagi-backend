namespace PostService.Domain.Entities
{
    public class Country:FullAuditableEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<City> Cities { get; set; } = [];
    }
}
