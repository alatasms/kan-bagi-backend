using FluentValidation;
using PostService.Application.Features.Commands;

namespace PostService.Application.Validators
{
    public class CreatePostCommandValidator: AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.PatientFullName)
                .NotEmpty().WithMessage("Patient name is required.")
                .MaximumLength(50).WithMessage("Patient name must not exceed 50 characters.");

            RuleFor(x => x.PatientAge)
                .NotNull().WithMessage("Patient age is required");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(20).WithMessage("Description must be at least 20 characters.")
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.PhoneNumbers)
                .NotNull().WithMessage("At least one phone number is required.")
                .Must(x => x.Count > 0).WithMessage("At least one phone number is required.")
                .Must(x => x.Count <= 3).WithMessage("Maximum number of pohone number is 2");

            RuleForEach(x => x.PhoneNumbers)
                .NotEmpty().WithMessage("Phone number cannot be empty.")
                .Matches(@"^(\+90|0)?5\d{9}$")
                .WithMessage("Phone number must be a valid Turkish mobile number (e.g. +905xx or 05xx).");

            RuleFor(x => x.BloodType)
                .NotNull().WithMessage("Blood type is required.")
                .IsInEnum().WithMessage("Invalid blood type selected.");

            RuleFor(x => x.HospitalId)
                .NotEmpty().WithMessage("Hospital ID is required.");
        }
    }
}
