using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnspotMechanicServiceApi.Models;
using OnSpotMechanicServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnspotMechanicServiceApi.Controllers
{
    [Route("api/service")]
    [ApiController]
    
    public class ServiceController : ControllerBase
    {
        ServiceDbContext context = new ServiceDbContext();


        [HttpPost]
        [Route("addCategory")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddServiceCategory(ServiceCategory serviceCategory)
        {
            
                context.ServiceCategories.Add(serviceCategory);
                context.SaveChanges();
                return Ok("Category Added");
        }

        [HttpPost]
        [Route("addServices/{serviceCategoryId}")]
       /* [Authorize(Roles = "Admin")]*/
        public IActionResult AddServices(Service service,int serviceCategoryId)
        {
            service.ServiceCategory = context.ServiceCategories.Find(serviceCategoryId);
            if (service.ServiceCategory != null)
            {
                context.Services.Add(service);
                context.SaveChanges();
                return Ok("Service Added");
            }
            else
                return NotFound("Category not exist");
        }

        [HttpGet]
        [Route("getCategories")]
        public IActionResult GetServiceCategories()
        {
            return Ok(context.ServiceCategories);

        }

        [HttpGet]
        [Route("getAllServices")]
        public IActionResult GetAllServices()
        {
            var services = context.Services.Include(d => d.ServiceCategory);
            return Ok(services);

        }

        [HttpGet]
        [Route("getServices/{categoryId}")]
        public IActionResult GetServices(int categoryId)
        {
            ServiceCategory category= context.ServiceCategories.Find(categoryId);
            var services = context.Services.Where(d => d.ServiceCategory == category);
            
            return Ok(services);

        }

        [HttpPut]
        [Route("updateServices/{serviceId}")]
        /*[Authorize(Roles = "Admin")]*/
        public IActionResult UpdateService(UpdateService updateService, int serviceId)
        {
            Service obj = context.Services.FirstOrDefault(d => d.ServiceId == serviceId);
            if (obj != null)
            {

                obj.ServiceName = updateService.ServiceName;
                obj.ServiceDetails = updateService.ServiceDetails;
                obj.ServiceCharge = updateService.ServiceCharge;

                context.Services.Update(obj);
                context.SaveChanges();

                return Ok("Service Updated");
            }
            else
                return NotFound("Service Not Found");
        }

        /*[Authorize(Roles = "Admin")]*/
        [Route("deleteService/{id}")]
        [HttpDelete]
        public IActionResult DeleteService(int serviceId)
        {
            Service service = context.Services.FirstOrDefault(d => d.ServiceId == serviceId);
            if (service != null)
            {
                context.Services.Remove(service);
                context.SaveChanges();
                return Ok("Service Deleted");
            }
            else
                return NotFound("Service Id not Found");

        }
    }
}
