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
    [Route("api/feedback")]
    [ApiController]
  /*  [Authorize]*/
    public class FeedbackController : ControllerBase
    {
        
            ServiceDbContext context = new ServiceDbContext();

           /* [Authorize(Roles = "Customer")]*/
            [HttpPost]
            [Route("addFeedback/{serviceBookingId}")]
            public IActionResult AddFeedback(Feedback feedback, int  serviceBookingId)
            {

                feedback.ServiceBooking = context.ServiceBookings.Find(serviceBookingId);
                context.Feedbacks.Add(feedback);
                context.SaveChanges();

                return Ok("Thanks for the Feedback");
             }

            
            [HttpGet]
            [Route("getAllFeedbacks")]
            public IActionResult DisplayFeedbacks()
            {
            var feedbacks = context.Feedbacks.Include(s => s.ServiceBooking.Customer).Include(s=>s.ServiceBooking.Service);
                return Ok(feedbacks);
            }
    }
}

