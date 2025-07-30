using System.Data;
using FluentValidation;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {

        RuleFor(x => x.Name).NotEmpty().WithMessage("İsim boş bırakılamaz!").MinimumLength(3).WithMessage("İsim 3 karakterden az olamaz.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyisim boş bırakılamaz!").MinimumLength(3).WithMessage("Soyisim 3 karakterden az olamaz");
        RuleFor(x => x.Age).NotEmpty().WithMessage("Yaş boş bırakılamaz!");
        RuleFor(x => x.Mail).NotEmpty().WithMessage("Mail boş bırakılamaz!").EmailAddress().WithMessage("Geçerli bir email girin.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre boş bırakılamaz!").MinimumLength(8).WithMessage("Şifreniz 8 karakterden az olamaz.");

    }
}