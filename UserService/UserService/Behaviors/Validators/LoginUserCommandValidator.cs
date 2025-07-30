using FluentValidation;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Mail).NotEmpty().WithMessage("Mail boş olamaz.").EmailAddress().WithMessage("Geçerli bir mail adresi girin.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre boş bırakılamaz").MinimumLength(4).WithMessage("Şifre 4 karakterden az olamaz");
    }
}