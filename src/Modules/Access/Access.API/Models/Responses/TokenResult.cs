﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models.Responses
{
    public record TokenResult(string Token, DateTime ExpiryDate);
}
