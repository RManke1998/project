using Microsoft.AspNetCore.Http;
using OnSpotMechanicServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnspotMechanicServiceApi.Models
{
    public class AuthRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        
        
    }

    

}
