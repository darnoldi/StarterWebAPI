﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterWebAPI.Identity.Entities
{
    public class AppRole : IdentityRole

    {
        public string Description { get; set; }
    }
}
