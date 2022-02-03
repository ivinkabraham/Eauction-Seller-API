using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eauction_Seller_API.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}
