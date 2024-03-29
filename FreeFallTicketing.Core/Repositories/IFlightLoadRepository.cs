﻿using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IFlightLoadRepository : IRepository<FlightLoad>
    {
        Task<FlightLoadItem?> GetFlightItemByTicket(Ticket ticket);
        Task<FlightLoad?> GetExpandedById(Guid id);
        Task UpdateFlightTicket(FlightLoadItem flightItem, Ticket ticket, SkyDiveEventTicketType ticketType, bool reservable);
        Task<FlightLoad?> GetFlightLoadByItem(FlightLoadItem flightLoadItem);
        void RemoveFlights(IEnumerable<FlightLoad> flights, SkyDiveEventItem skyDiveEventDay);
        Task RemoveTicket(FlightLoadItem flightLoadItem, Ticket ticket);
        void RemoveFlight(SkyDiveEventItem skyDiveEventItem, FlightLoad flight);
        void SetFlightStatus(FlightLoad flight, FlightStatus status);
        void SetFlightName(FlightLoad flight, string name);
    }
}
