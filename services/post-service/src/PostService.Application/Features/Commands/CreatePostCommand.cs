using MediatR;
using PostService.Application.Features.Common;
using PostService.Domain.BusinessModels;

namespace PostService.Application.Features.Commands
{
    public class CreatePostCommand: IRequest<StandardResponse<PostResponse>>
    {
        public string PatientFullName { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> PhoneNumbers { get; set; } = [];
        public BloodType BloodType { get; set; }
        public int HospitalId { get; set; }
    }
}
