using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnspotMechanicServiceApi.Models;
using OnSpotMechanicServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnspotMechanicServiceApi.Controllers
{
   
    public abstract class BaseController : ControllerBase
    {
        public ServiceDbContext context;
        public BaseController(ServiceDbContext context)
        {
            this.context = context;
        }

        [NonAction]
        public Customer GetCurrentCustomer()
        {
            var customerId = HttpContext.User.Claims
                .FirstOrDefault(d => d.Type ==
                ClaimTypes.NameIdentifier).Value;

            Customer customer = context.Customers.FirstOrDefault(u => u.CustomerId == int.Parse(customerId));
            return customer;
        }

        public Mechanic GetCurrentMechanic()
        {
            var mechanicId = HttpContext.User.Claims
                .FirstOrDefault(d => d.Type ==
                ClaimTypes.NameIdentifier).Value;

            Mechanic mechanic = context.Mechanics.FirstOrDefault(u => u.MechanicId == int.Parse(mechanicId));
            return mechanic;
        }


        public void UpdateMechStatus(UpdateMechanicStatus updateMechanicStatus, int mechanicId)
        {
            Mechanic mechanic = context.Mechanics.FirstOrDefault(u => u.MechanicId == mechanicId);
            if (mechanic != null)
            {
                mechanic.MechanicStatus = updateMechanicStatus.MechanicStatus;
                context.Mechanics.Update(mechanic);
                context.SaveChanges();
            }
        }

        

    }
}
