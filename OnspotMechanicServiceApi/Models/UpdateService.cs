using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnspotMechanicServiceApi.Models
{
    public class UpdateService
    {
        public string ServiceName { get; set; }
        public string ServiceDetails { get; set; }
        public float ServiceCharge { get; set; }
    }
}
