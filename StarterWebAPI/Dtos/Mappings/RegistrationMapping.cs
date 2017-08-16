using AutoMapper;
using StarterWebAPI.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterWebAPI.Dtos.Mappings
{
    public class RegistrationMapping : Profile
    {
        public RegistrationMapping()
        {
            CreateMap<RegistrationDto, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        }
    }
}
