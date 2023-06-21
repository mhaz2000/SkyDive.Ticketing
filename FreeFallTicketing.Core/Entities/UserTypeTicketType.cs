using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Core.Entities
{
    public class UserTypeTicketType
    {
        public UserType UserType { get; set; }
        public Guid UserTypeId { get; set; }

        public SkyDiveEventTicketType TicketType { get; set; }
        public Guid TicketTypeId { get; set; }
    }
}
