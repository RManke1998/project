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
    [Route("api/serviceBooking")]
    [ApiController]
   /* [Authorize]*/
    public class ServiceBookingController : BaseController
    {
       
        public ServiceBookingController():base(new ServiceDbContext())
        {}
       

        /*[Authorize(Roles = "Customer")]*/
        [HttpPost]
        [Route("addBooking/{serviceId}")]
        public IActionResult AddBooking(ServiceBooking serviceBooking,int serviceId)
        {
            AllocateMechanicService obj = new AllocateMechanicService();
            int mechanicId=obj.Allocate(serviceBooking);
            if (mechanicId != 0)
            {
                UpdateMechanicStatus data = new UpdateMechanicStatus();
                data.MechanicStatus = "Assigned";
                UpdateMechStatus(data, mechanicId);
                serviceBooking.Mechanic = context.Mechanics.Find(mechanicId);
                serviceBooking.Customer = GetCurrentCustomer();
                serviceBooking.Service = context.Services.Find(serviceId);
                context.ServiceBookings.Add(serviceBooking);
                context.SaveChanges();


                return Ok("Service Booked");
            }
            else
                return NotFound("Mechanic not available");
        }

        [HttpPut]
        [Route("updateServiceStatus/{bookingId}/{serviceId}")]
        /*[Authorize(Roles ="Mechanic")]*/
        public IActionResult UpdateStatus(UpdateBookingStatus updateBookingStatus,int bookingId,int serviceId)
        {
            ServiceBooking serviceBooking=context.ServiceBookings.Find(bookingId);
            serviceBooking.ServiceStatus = updateBookingStatus.ServiceStatus;
            context.ServiceBookings.Update(serviceBooking);
            context.SaveChanges();

            UpdateMechanicStatus data = new UpdateMechanicStatus();
            data.MechanicStatus = "Not Assigned";
            Mechanic mechanic = GetCurrentMechanic();
            UpdateMechStatus(data, mechanic.MechanicId) ;

            
            
            GenerateInvoiceService obj = new GenerateInvoiceService();
            obj.GenerateInvoice(serviceId,bookingId);


            return Ok("Updated");

        }



       /* [Authorize(Roles = "Admin")]*/
        [Route("getAllBookings")]
        [HttpGet]
        public IActionResult GetAllBookings()
        {
            /* List<Customer> customers = context.Customers.ToList();*/
            var booking = context.ServiceBookings.Include(s => s.Service).Include(s=>s.Customer).Include(s=>s.Mechanic);
            return Ok(booking);
        }

        /*[Authorize(Roles = "Customer")]*/
        [Route("getBooking")]
        [HttpGet]
        public IActionResult GetBooking()
        {
            Customer customer = GetCurrentCustomer();
            var bookingDetails = context.ServiceBookings.Where(d => d.Customer == customer);

            if (bookingDetails != null)
            {
                return Ok(bookingDetails);
            }
            else
                return NotFound("Customer not Have any Bookings");

        }

       /* [Authorize(Roles = "Customer")]*/
        [Route("deleteBooking/{bookingId}")]
        [HttpDelete]
        public IActionResult DeleteCustomer(int bookingId)
        {
            ServiceBooking serviceBooking = context.ServiceBookings.FirstOrDefault(d => d.ServiceBookingId == bookingId);
            if (serviceBooking != null)
            {
                context.ServiceBookings.Remove(serviceBooking);
                context.SaveChanges();
                return Ok("Booking Deleted");
            }
            else
                return NotFound("Booking Id not Found");

        }

       
    }
}
