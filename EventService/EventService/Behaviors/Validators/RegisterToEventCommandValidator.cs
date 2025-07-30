using FluentValidation;

public class RegisterToEventCommandValidator : AbstractValidator<RegisterToEventCommand>
{
    public RegisterToEventCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId boş olamaz.");
        RuleFor(x => x.EventId).NotEmpty().WithMessage("EventId boş olamaz.");
    }
}
