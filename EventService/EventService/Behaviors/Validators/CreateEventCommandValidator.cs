using System.Data;
using FluentValidation;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş bırakılamaz.");
        RuleFor(x => x.Location).NotEmpty().WithMessage("Konum boş bırakılamaz.");
        RuleFor(x => x.StartDate).GreaterThanOrEqualTo(DateTime.Today).WithMessage("Bitiş tarihi başlangıç tarihinden büyük olamaz.");
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage("Bitiş tarhi başlangıç tarihinden önce olamaz.");
        RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("Kapasite minimum 1 olmalıdır.");
    }
}