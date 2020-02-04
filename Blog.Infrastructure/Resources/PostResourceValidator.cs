using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Infrastructure.Resources
{
    public class PostResourceValidator : AbstractValidator<PostResource>
    {
        public PostResourceValidator()
        {
            RuleFor(x => x.Author)
                    .NotNull()
                    .WithName("Author")
                    .WithMessage("{PropertyName} is required")
                    .MaximumLength(50)
                    .WithMessage("{PropertyName} should be less than 50 characters");
        }
    }
}
