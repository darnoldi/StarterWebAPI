﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterAPI.Entities
{
    public class Customer
    {
        public Guid  Id { get; set; }
        public string Firstname { get; set; }
        public string  Lastname { get; set; }
        public int Age { get; set; }
    }
}
