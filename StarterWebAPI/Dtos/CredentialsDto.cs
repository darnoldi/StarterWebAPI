using FluentValidation.Attributes;
using StarterWebAPI.Dtos.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterWebAPI.Dtos
{   [Validator(typeof(CredentialsValidator))]
    public class CredentialsDto
    {
               
       
            public string UserName { get; set; }
            public string Password { get; set; }
        
    }
}
