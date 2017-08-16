using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterWebAPI.Dtos.Validators
{
    public class CredentialsValidator : AbstractValidator<CredentialsDto>
    {
        public CredentialsValidator()
        {
            RuleFor(vm => vm.UserName).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(vm => vm.Password).Length(6, 12).WithMessage("Password must be between 6 and 12 characters");
        }
    }

    }
