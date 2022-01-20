using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eauction_Seller_API.Models
{
    public class BidDetails
    {
        public Product ProductInfo { get; set; }

        public List<Bid> BidList { get; set; }
    }
}
