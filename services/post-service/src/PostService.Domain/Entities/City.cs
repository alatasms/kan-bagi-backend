using System.ComponentModel.DataAnnotations.Schema;

namespace PostService.Domain.Entities
{
    public class City:FullAuditableEntity<int>
    {
        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public ICollection<District> Districts { get; set; } = [];
    }
}
