using MediatR;
using PostService.Application.Features.Common;
using PostService.Domain.BusinessModels;

namespace PostService.Application.Features.Commands
{
    /// <summary>Editable fields of a post. Owner, activeness and expiry are not changed here.</summary>
    public class UpdatePostCommand: IRequest<StandardResponse<PostResponse>>
    {
        public Guid Id { get; set; }
        public string PatientFullName { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> PhoneNumbers { get; set; } = [];
        public BloodType BloodType { get; set; }
        public int HospitalId { get; set; }
    }
}
