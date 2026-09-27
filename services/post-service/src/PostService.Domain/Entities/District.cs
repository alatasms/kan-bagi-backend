using System.ComponentModel.DataAnnotations.Schema;

namespace PostService.Domain.Entities
{
    public class District: FullAuditableEntity<int>
    {
        public int CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public City City { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public ICollection<Hospital> Hospitals { get; set; } = [];
    }
}
