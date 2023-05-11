using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnspotMechanicServiceApi.Models;
using OnSpotMechanicServiceLibrary;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnspotServiceApi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        ServiceDbContext context = new ServiceDbContext();

       

        [HttpPost]
        public IActionResult Login(AuthRequest authRequest)
        {
            var customer =context.Customers.FirstOrDefault(d => d.Email == authRequest.Email && d.Password == authRequest.Password);
            var admin = context.Admins.FirstOrDefault(d => d.Email == authRequest.Email && d.Password == authRequest.Password);
            var mechanic = context.Mechanics.FirstOrDefault(d => d.Email == authRequest.Email && d.Password == authRequest.Password);


            if (customer != null)
            {
                var jwttoken = GenerateToken(customer.Email, customer.Role, customer.CustomerId);
                
                return Ok(customer.Role+jwttoken);
            }
            else if(admin != null)
            {
                var jwttoken = GenerateToken(admin.Email, admin.Role, admin.AdminId);

                return Ok(admin.Role+jwttoken);
            }
        
            else if(mechanic!=null)
            {
             var jwttoken = GenerateToken(mechanic.Email, mechanic.Role, mechanic.MechanicId);
                
                return Ok(mechanic.Role+jwttoken);
            }


            else
                return Unauthorized("Login Failed");

        }

        private string GenerateToken(string email,string role,int id)
        {
            string jwtToken = string.Empty;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Some Very Very Secret Key"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role,role ));

            var token = new JwtSecurityToken("myserviceapp.com", "myserviceapp.com", claims, expires: DateTime.Now.AddDays(7)
                                            , signingCredentials: credentials);


            jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
