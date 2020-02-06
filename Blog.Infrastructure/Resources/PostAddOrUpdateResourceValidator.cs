using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Infrastructure.Resources
{
    public class PostAddOrUpdateResourceValidator<T> : AbstractValidator<T> where T : PostAddOrUpdateResource
    {
        public PostAddOrUpdateResourceValidator()
        {
            RuleFor(x => x.Title)
                    .NotNull()
                    .WithName("Title")
                    .WithMessage("required|{PropertyName} is required")
                    .MaximumLength(50)
                    .WithMessage("maxlength|{PropertyName} should be less than 50 characters");

            RuleFor(x => x.Body)
                   .NotNull()
                   .WithName("Body")
                   .WithMessage("required|{PropertyName} is required")
                   .MaximumLength(100)
                   .WithMessage("maxlength|{PropertyName} should be less than 100 characters");
        }
    }
}
