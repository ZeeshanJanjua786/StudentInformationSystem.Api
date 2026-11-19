using FluentValidation;
using Sis.DTOs;
using Sis.Models;

namespace Sis.Validator
{
    public class CourseValidator : AbstractValidator<CourseRequestDto>
    {
        public CourseValidator()
        {
            RuleFor(c => c.Code)
                .Matches(@"^[A-Z]{2,4}[0-9]{3}")
                .WithMessage("Invalide Code");
            RuleFor(c => c.Title)
                .Length(3, 100)
                .WithMessage("Title must withing 3 to 100 characters");

            RuleFor(c => c.CreditHours)
                .LessThanOrEqualTo(4)
                .GreaterThan(1)
                .WithMessage("it must be within 1 to 4");
            RuleFor(c => c.Capacity)
                .LessThanOrEqualTo(200)
                .GreaterThan(1)
                .WithMessage("it must be within 1 to 200");

        }
        public class UpdateCourseValidator : AbstractValidator<updateRequestDto>
        {
            public UpdateCourseValidator()
            {

                RuleFor(c => c.Title)
                    .Length(3, 100)
                    .WithMessage("Title must withing 3 to 100 characters");

                RuleFor(c => c.CreditHours)
                    .LessThanOrEqualTo(4)
                    .GreaterThan(1)
                    .WithMessage("it must be within 1 to 4");
                RuleFor(c => c.Capacity)
                    .LessThanOrEqualTo(200)
                    .GreaterThan(1)
                    .WithMessage("it must be within 1 to 200");

            }

        }

    

    }
}
