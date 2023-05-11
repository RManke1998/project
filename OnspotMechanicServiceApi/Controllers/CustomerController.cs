using Microsoft.AspNetCore.Authorization;
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
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        ServiceDbContext context = new ServiceDbContext();

        [HttpPost]
        [Route("addCustomer")]
        public IActionResult AddCustomer(Customer customer)
        {
            if (!context.Customers.Any(d => d.Email == customer.Email))
            {
                
               /* customer.Gender=(GenderValue)Enum.Parse(typeof(GenderValue),);*/
                context.Customers.Add(customer);
                context.SaveChanges();
                return Ok("Customer Added");
            }
            else
                return BadRequest("Customer with this email already exist");
        }

       /* [Authorize(Roles= "Admin")]*/
        [Route("getAllCustomers")]
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
           /* List<Customer> customers = context.Customers.ToList();*/

            return Ok(context.Customers);
        }

        //chnge in method
        /*[Authorize(Roles = "Customer")]*/
        [Route("getCustomer/{id}")]
        [HttpGet]
        public IActionResult GetCustomer(int id)
        {
            Customer customer = context.Customers.FirstOrDefault(d => d.CustomerId == id);

            if (customer != null)
            {
                return Ok(customer);
            }
            else
                return BadRequest("Customer not exist");

        }

        /*[Authorize(Roles = "Customer")]*/
        [Route("updateCustomer/{id}")]
        [HttpPut]
        public IActionResult UpdateCustomer(UpdateProfile updateProfile,int id)
        {
            Customer obj = context.Customers.FirstOrDefault(d => d.CustomerId ==id);
            if (obj != null)
            {
               
                obj.Email = updateProfile.Email;
                obj.Password = updateProfile.Password;
                obj.ContactNo = updateProfile.ContactNo;

                context.Customers.Update(obj);
                context.SaveChanges();

                return Ok("Customer Updated");
            }
            else
                return NotFound("Customer Not Found");
        }

       
        /* public void DeleteCustomer(int customerid)
         {
             Customer customer = context.Customers.FirstOrDefault(d => d.CustomerId == customerid);
             if (customer != null)
             {
                 context.Customers.Remove(customer);
                 context.SaveChanges();
             }
             else
                 throw new Exception($"Customer with the id: {customerid} does not exist");

         }*/

    }
}
