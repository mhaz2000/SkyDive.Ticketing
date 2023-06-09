﻿using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class SkyDiveEventTicketType : BaseEntity
    {
        public SkyDiveEventTicketType()
        {

        }

        public SkyDiveEventTicketType(string title, int capacity) : base()
        {
            Title = title;
            Capacity = capacity;
        }

        public string Title { get; set; }
        public int Capacity { get; set; }

        public ICollection<UserTypeTicketType> AllowedUserTypes { get; set; }
    }
}
