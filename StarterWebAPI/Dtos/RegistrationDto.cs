using FluentValidation.Attributes;
using StarterWebAPI.Dtos.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterWebAPI.Dtos
{[Validator(typeof(RegistrationValidator))]
    public class RegistrationDto
    {
        
            public string Email { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Location { get; set; }
       }
    }




