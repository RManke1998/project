using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnspotMechanicServiceApi.Models;
using OnSpotMechanicServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnspotMechanicServiceApi.Controllers
{
    [Route("api/mechanic")]
    [ApiController]
    public class MechanicController : BaseController
    {
        public MechanicController() : base(new ServiceDbContext())
        { }

        /*[Authorize(Roles = "Admin")]*/
        [HttpPost]
        [Route("addMechanic")]
        public IActionResult AddMechanic(Mechanic mechanic)
        {
            if (!context.Customers.Any(d => d.Email == mechanic.Email))
            {
                context.Mechanics.Add(mechanic);
                context.SaveChanges();
                return Ok("Mechanic Added");
            }
            else
                return Ok("Mechanic with this email already exist");
        }

        /*[Authorize(Roles = "Admin")]*/
        [Route("getMechanics")]
        [HttpGet]
        public IActionResult GetAllMechanic()
        {
           
            return Ok(context.Mechanics);
        }

       /* [Authorize(Roles = "Mechanic")]*/
        [Route("getMechanic")]
        [HttpGet]
        public IActionResult GetMechanic()
        {
            Mechanic mechanic = GetCurrentMechanic();

            if (mechanic != null)
            {
                return Ok(mechanic);
            }
            else
                return Ok("Mechanic not exist");

        }

        /*[Authorize(Roles = "Mechanic")]*/
        [Route("updateMechanic/{id}")]
        [HttpPut]
        public IActionResult UpdateMechanic(UpdateProfile updateProfile,int id)
        {
            Mechanic obj = context.Mechanics.FirstOrDefault(d => d.MechanicId == id);
            if (obj != null)
            {
                obj.Email = updateProfile.Email;
                obj.Password = updateProfile.Password;
                obj.ContactNo = updateProfile.ContactNo;

                context.Mechanics.Update(obj);
                context.SaveChanges();

                return Ok("Mechanic Updated");
            }
            else
                return NotFound("Mechanic Not Found");
        }

        /*[Authorize(Roles = "Admin")]*/
        [Route("deleteMechanic/{id}")]
        [HttpDelete]
        public IActionResult DeleteCustomer(int mechanicId)
         {
             Mechanic mechanic = context.Mechanics.FirstOrDefault(d => d.MechanicId == mechanicId);
            if (mechanic != null)
            {
                context.Mechanics.Remove(mechanic);
                context.SaveChanges();
                return Ok("Mechanic Deleted");
            }
            else
                return NotFound("Mechanic with Id not Found");

         }

        /*[Authorize(Roles = "Mechanic")]*/
        [Route("getBooking")]
        [HttpGet]
        public IActionResult GetBooking()
        {
            Mechanic mechanic = GetCurrentMechanic();
            var bookingDetails = context.ServiceBookings.Where(d => d.Mechanic ==mechanic ).Include(s=>s.Service).Include(s=>s.Customer);

            if (bookingDetails != null)
            {
                return Ok(bookingDetails);
            }
            else
                return NotFound("No Booking Assigned");

        }

    }
}
