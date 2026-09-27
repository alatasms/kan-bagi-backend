using UserService.Application.Features.Commands;
using FluentValidation;
using Microsoft.Extensions.Options;
using UserService.Infrastructure.Services.IdentityVerification;

namespace UserService.Application.Validators
{
    public class RegisterCommandValidator : AbstractValidator<CompleteProfileCommand>
    {
        public RegisterCommandValidator(IOptions<IdentityVerificationOptions> identityOptions)
        {

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon numarası boş olamaz.")
                .Matches(@"^(\+90|0)?5\d{9}$").WithMessage("Geçerli bir Türkiye telefon numarası giriniz.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ad boş olamaz.")
                .MinimumLength(2).WithMessage("Ad en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir.")
                .Matches("^[a-zA-ZğüşıöçĞÜŞİÖÇ ]+$").WithMessage("Ad yalnızca harf ve boşluk içerebilir.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Soyad boş olamaz.")
                .MinimumLength(2).WithMessage("Soyad en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir.")
                .Matches("^[a-zA-ZğüşıöçĞÜŞİÖÇ ]+$").WithMessage("Soyad yalnızca harf ve boşluk içerebilir.");

            RuleFor(x => x.TCIdentityNumber)
                .NotEmpty().WithMessage("T.C. Kimlik Numarası boş olamaz.")
                .Length(11).WithMessage("T.C. Kimlik Numarası 11 haneli olmalıdır.")
                .Matches("^[1-9][0-9]{10}$").WithMessage("Geçersiz T.C. Kimlik Numarası formatı.")
                .When(_ => identityOptions.Value.Enabled);

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Doğum tarihi boş olamaz.")
                .Must(BeInThePast).WithMessage("Doğum tarihi geçmişte olmalıdır.")
                .Must(BeAtLeast18YearsOld).WithMessage("Kullanıcı 18 yaşından büyük olmalıdır.");

            RuleFor(x => x.BloodType)
                .NotNull()
                .IsInEnum().WithMessage("Geçersiz kan grubu.");
            RuleFor(x => x.Gender)
                .NotNull()
                .IsInEnum().WithMessage("Geçersiz cinsiyet tipi.");
        }

        private bool BeInThePast(DateOnly birthDate)
        {
            return birthDate < DateOnly.FromDateTime(DateTime.Today);
        }

        private bool BeAtLeast18YearsOld(DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
            {
                age--;
            }

            return age >= 18;
        }
    }
}
