using System.ComponentModel.DataAnnotations.Schema;

namespace PostService.Domain.Entities
{
    public class Hospital: FullAuditableEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? WebSite { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public int DistrictId { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public District District { get; set; } = null!;
    }
}
