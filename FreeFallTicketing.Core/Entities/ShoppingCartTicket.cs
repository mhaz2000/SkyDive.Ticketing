using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Core.Entities
{
    public class ShoppingCartTicket
    {
        public Ticket Ticket { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        public Guid TicketId { get; set; }
        public Guid ShoppingCartId { get; set; }
    }
}
