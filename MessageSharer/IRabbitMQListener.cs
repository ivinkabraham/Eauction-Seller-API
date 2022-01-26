using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eauction_Seller_API.MessageSharer
{
    public interface IRabbitMQListener
    {
            void Receive();
    }
}
