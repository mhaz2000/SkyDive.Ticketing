﻿using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using SkyDiveTicketing.Application.DTOs.FlightLoadDTOs;
using SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs;
using SkyDiveTicketing.Application.DTOs.TicketDTOs;

namespace SkyDiveTicketing.Application.Services.SkyDiveEventServices
{
    public interface ISkyDiveEventService
    {
        Task Create(SkyDiveEventCommand command);
        Task<SkyDiveEventDTO> GetEvent(Guid id);
        IEnumerable<SkyDiveEventDTO> GetEvents(Guid? statusId, DateTime? start, DateTime? end);
        Task Remove(Guid id);
        Task Update(SkyDiveEventCommand command, Guid id);
        Task ToggleActivationEvent(Guid id);
        Task AddFlight(AddSkyDiveEventFlightCommand command, Guid id);
        string GetLastCode(Guid id);
        Task<IEnumerable<SkyDiveEventDaysDTO>> GetEventDays(Guid id, Guid userId);
        Task AddEventTypeFee(AddEventTypeFeeCommand command, Guid id);
        Task AddConditionsAndTerms(AddEventConditionsAndTermsCommand command, Guid id);
        SkyDiveEventItemDTO GetEventDayFlights(Guid id, int pageSize, int pageIndex);
        Task<IEnumerable<FlightLoadItemTicketDTO>> GetFlightTickets(Guid id);
        Task UpdateTicket(UpdateTicketCommand command);
        Task PublishEvent(Guid id);
        (ReservingTicketDTO reservingTicketDTO, int count) GetEventDayTickets(Guid id, int index, int size);
    }
}
