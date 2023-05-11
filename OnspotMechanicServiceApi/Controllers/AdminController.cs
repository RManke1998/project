using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnSpotMechanicServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnspotMechanicServiceApi.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        ServiceDbContext context = new ServiceDbContext();

        [Authorize(Roles = "Admin")]
        [Route("getAdmin")]
        [HttpGet]
        public IActionResult GetAdmin()
        {
            Admin admin = GetCurrentAdmin();

            if (admin != null)
            {
                return Ok(admin);
            }
            else
                return Ok("admin not exist");
        }

        [HttpPost]
        [Route("addAdmin")]
        public IActionResult AddAdmin(Admin admin)
        {
            if (!context.Admins.Any(d => d.Email == admin.Email))
            {

                /* customer.Gender=(GenderValue)Enum.Parse(typeof(GenderValue),);*/
                context.Admins.Add(admin);
                context.SaveChanges();
                return Ok("Admin Added");
            }
            else
                return Ok("Admin with this email already exist");
        }

        /*[Authorize(Roles = "Admin")]
        [Route("updateAdmin/{id}")]
        [HttpPut]
        public IActionResult UpdateAdmin(,int id)
        {
            Admin adminObj = context.Admins.FirstOrDefault(d => d.AdminId == id);

            if (adminObj != null)
            {
                adminObj.Name = admin.Name;
                adminObj.Email = admin.Email;
                adminObj.Password = admin.Password;

                context.Admins.Update(adminObj);
                context.SaveChanges();

                return Ok("Admin Details Updated");
            }
            else
                return NotFound("admin not exist");
        }*/

        [Authorize(Roles = "Admin")]
        [Route("deleteAdmin/{id}")]
        [HttpDelete]
        public IActionResult DeleteAdmin(int id)
        {
            Admin admin = context.Admins.FirstOrDefault(d => d.AdminId == id);
            if (admin != null)
            {
                context.Admins.Remove(admin);
                context.SaveChanges();
                return Ok("Admin Deleted");
            }
            else
                return NotFound($"Admin with the id:{id} does not exist");
        }

        public Admin GetCurrentAdmin()
        {
            var adminId = HttpContext.User.Claims
                .FirstOrDefault(d => d.Type ==
                ClaimTypes.NameIdentifier).Value;

            Admin admin = context.Admins.FirstOrDefault(u => u.AdminId == int.Parse(adminId));
            return admin;
        }
    }
}
