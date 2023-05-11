using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSpotMechanicServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnspotMechanicServiceApi.Controllers
{
    [Route("api/invoice")]
    [ApiController]

    public class InvoiceController : ControllerBase
    {
        
        /*public InvoiceController(ServiceDbContext context) : base(context)
        {
            this.context = new ServiceDbContext();
        }
*/
       ServiceDbContext context=new ServiceDbContext();

        /*[Authorize(Roles = "Customer")]*/
        [Route("getInvoice/{serviceBookingId}")]
        [HttpGet]
        public IActionResult GetInvoice(int serviceBookingId)
        {
            
            ServiceBooking serviceBooking = context.ServiceBookings.Find(serviceBookingId);
            Invoice invoice = context.Invoices.FirstOrDefault(d => d.ServiceBooking == serviceBooking);
            if (invoice != null)
            {
                return Ok(invoice);
            }
            else
                return NotFound("No invoice found");

        }

       /* [Authorize(Roles = "Admin")]*/
        [Route("getAllInvoices")]
        [HttpGet]
        public IActionResult GetAllInvoices()
        {
            var invoices = context.Invoices.Include(s => s.ServiceBooking).Include(s=>s.ServiceBooking.Service);

            return Ok(invoices);
        }

        
    }
}
