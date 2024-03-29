﻿using Microsoft.EntityFrameworkCore;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Model;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;
using System.Linq;
using System.Linq.Expressions;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class SkyDiveEventRepository : Repository<SkyDiveEvent>, ISkyDiveEventRepository
    {
        public SkyDiveEventRepository(DataContext context) : base(context)
        {
        }

        public void AddConditionsAndTerms(SkyDiveEvent skyDiveEvent, string conditionsAndTerms)
        {
            skyDiveEvent.TermsAndConditions = conditionsAndTerms;
        }

        public async Task AddFlightsAsync(SkyDiveEvent skyDiveEvent, SkyDiveEventItem skyDiveEventDay, IDictionary<SkyDiveEventTicketType, int> typesQty, int flightQty, int voidableQty, int capacity)
        {
            var lastTicketNumber = skyDiveEvent.Items.SelectMany(s => s.FlightLoads, (skyDiveEventItem, flightLoad) => flightLoad)?
                .SelectMany(c => c.FlightLoadItems, (flightLoad, flightLoadItem) => flightLoadItem)?
                .SelectMany(c => c.Tickets, (item, ticket) => ticket)?.OrderByDescending(c => c.TicketNumber)?.FirstOrDefault()?.TicketNumber;

            var ticketCounter = !string.IsNullOrEmpty(lastTicketNumber) ? int.Parse(lastTicketNumber.Substring(lastTicketNumber.Count() - 4)) : 0;

            var lastNumber = skyDiveEvent.Items.SelectMany(s=> s.FlightLoads).OrderByDescending(s => s.Number).FirstOrDefault()?.Number ?? 0;
            for (int i = 0; i < flightQty; i++)
            {
                var flightLoad = new FlightLoad(++lastNumber, skyDiveEventDay.Date, capacity, voidableQty);

                foreach (var item in typesQty)
                {
                    var entity = new FlightLoadItem(item.Key, item.Value);
                    var tickets = await GenerateTickets(item.Value, lastNumber, skyDiveEvent, ticketCounter);
                    ticketCounter += tickets.Count;
                    entity.Tickets = tickets;

                    await Context.FlightLoadItems.AddAsync(entity);
                    flightLoad.FlightLoadItems.Add(entity);
                }

                skyDiveEventDay.FlightNumber = flightQty;

                await Context.FlightLoads.AddAsync(flightLoad);
                skyDiveEventDay.FlightLoads.Add(flightLoad);
            }
        }

        public async Task AddTicketTypeAmount(SkyDiveEvent skyDiveEvent, SkyDiveEventTicketType ticketType, double amount)
        {
            var entity = new SkyDiveEventTicketTypeAmount(ticketType, amount);
            await Context.SkyDiveEventTicketTypeAmounts.AddAsync(entity);

            skyDiveEvent.TypesAmount.Add(entity);
        }

        public void ClearTicketTypesAmount(SkyDiveEvent skyDiveEvent)
        {
            skyDiveEvent.TypesAmount.Clear();
        }

        public async Task Create(string title, string location, bool voidable, bool subjecToVAT, Guid? image,
            DateTime startDate, DateTime endDate, SkyDiveEventStatus status)
        {
            var lastCode = Context.SkyDiveEvents.OrderByDescending(x => x.Code).FirstOrDefault()?.Code ?? 0;
            var skyDiveEvent = new SkyDiveEvent(++lastCode, title, location, voidable, subjecToVAT, image, startDate, endDate, status);

            for (int i = 0; i < (endDate - startDate).TotalDays + 1; i++)
            {
                var entity = new SkyDiveEventItem(startDate.AddDays(i));
                await Context.SkyDiveEventItems.AddAsync(entity);

                skyDiveEvent.Items.Add(entity);
            }

            await Context.SkyDiveEvents.AddAsync(skyDiveEvent);
        }

        public IQueryable<SkyDiveEvent> FindEvents(Expression<Func<SkyDiveEvent, bool>>? predicate = null)
        {
            var events = Context.SkyDiveEvents
                .Include(c => c.TypesAmount).ThenInclude(c => c.Type)
                .Include(c => c.Status)
                .Include(c => c.Items).ThenInclude(c => c.FlightLoads).ThenInclude(c => c.FlightLoadItems).ThenInclude(c => c.FlightLoadType)
                .Include(c => c.Items).ThenInclude(c => c.FlightLoads).ThenInclude(c => c.FlightLoadItems).ThenInclude(c => c.Tickets).ThenInclude(c => c.ReservedBy)
                .Include(c => c.Items).ThenInclude(c => c.FlightLoads).ThenInclude(c => c.FlightLoadItems).ThenInclude(c => c.Tickets).ThenInclude(c => c.ReservedFor).ThenInclude(c => c.Passenger)
                .Include(c => c.Items).ThenInclude(c => c.FlightLoads).ThenInclude(c => c.FlightLoadItems).ThenInclude(c => c.Tickets).AsQueryable();

            if (predicate is not null)
                events = events.Where(predicate).AsQueryable();

            return events;
        }

        public async Task<IEnumerable<TicketDetailModel>> GetExpandedSkyDiveEvent(Guid id)
        {
            var skyDiveEvent = await FindEvents().FirstOrDefaultAsync(e => e.Id == id);

            return skyDiveEvent.Items.SelectMany(c => c.FlightLoads, (skyDiveEventItem, flightLoad) => new { skyDiveEventItem, flightLoad })
                   .SelectMany(c => c.flightLoad.FlightLoadItems, (flightLoad, flightLoadItems) => new { flightLoad.flightLoad, flightLoad.skyDiveEventItem, flightLoadItems })
                   .Select(c => new TicketDetailModel
                   {
                       SkyDiveEvent = skyDiveEvent,
                       SkyDiveEventItem = c.skyDiveEventItem,
                       FlightLoad = c.flightLoad,
                       FlightLoadItem = c.flightLoadItems,
                   });
        }

        public async Task<bool> HasFlightLoad(Guid id)
        {
            var skyDiveEvent = await Context.SkyDiveEvents.Include(c => c.Items).ThenInclude(c => c.FlightLoads).FirstOrDefaultAsync(c => c.Id == id);
            if (skyDiveEvent is null)
                return false;

            return skyDiveEvent.Items.Any(c => c.FlightLoads.Any());
        }

        public void PublishEvent(SkyDiveEvent skyDiveEvent)
        {
            skyDiveEvent.ToggleActivation();
        }

        public void ToggleActivation(SkyDiveEvent skyDiveEvent)
        {
            skyDiveEvent.ToggleActivation();
        }

        public async Task Update(string title, string location, bool voidable, bool subjecToVAT, Guid? image, DateTime startDate,
            DateTime endDate, SkyDiveEventStatus status, SkyDiveEvent skyDiveEvent)
        {
            Context.SkyDiveEventItems.RemoveRange(skyDiveEvent.Items);

            skyDiveEvent.Status = status;
            skyDiveEvent.Title = title;
            skyDiveEvent.Location = location;
            skyDiveEvent.Voidable = voidable;
            skyDiveEvent.SubjecToVAT = subjecToVAT;
            skyDiveEvent.Image = image;
            skyDiveEvent.StartDate = startDate;
            skyDiveEvent.EndDate = endDate;

            for (int i = 0; i < (endDate - startDate).TotalDays + 1; i++)
            {
                var entity = new SkyDiveEventItem(startDate.AddDays(i));
                await Context.SkyDiveEventItems.AddAsync(entity);

                skyDiveEvent.Items.Add(entity);
            }
        }

        public async Task<IEnumerable<SkyDiveEventTicketTypeAmount>?> GetSkyDiveEventTicketTypesAmount(Guid id)
        {
            var skyDiveEvent = await Context.SkyDiveEvents.Include(c => c.TypesAmount).ThenInclude(c => c.Type).FirstOrDefaultAsync(c => c.Id == id);

            return skyDiveEvent?.TypesAmount;
        }

        private async Task<IList<Ticket>> GenerateTickets(int ticketQty, int flightNumber, SkyDiveEvent skyDiveEvent, int counter)
        {
            List<Ticket> tickets = new List<Ticket>();
            for (int i = 0; i < ticketQty; i++)
            {
                counter++;

                var number = skyDiveEvent.Code.ToString("000") + flightNumber.ToString("000") + counter.ToString("0000");

                var ticket = new Ticket(number, skyDiveEvent.Voidable, null, false);

                tickets.Add(ticket);

                await Context.Tickets.AddAsync(ticket);
            }

            return tickets;
        }

        public async Task<IEnumerable<FlightLoad>?> GetSkyDiveEventDayFlights(Guid id)
        {
            return (await Context.SkyDiveEventItems.Include(c => c.FlightLoads).ThenInclude(c => c.FlightLoadItems).ThenInclude(c => c.Tickets)
                .FirstOrDefaultAsync(c => c.Id == id))?.FlightLoads;
        }

        public Task<bool> CheckIfCriticalDataIsChanged(SkyDiveEvent skyDiveEvent, string title, string location, bool voidable,
            bool subjecToVAT, DateTime startDate, DateTime endDate)
        {
            return Task.FromResult(skyDiveEvent.Title != title || skyDiveEvent.Location != location || skyDiveEvent.Voidable != voidable ||
                skyDiveEvent.SubjecToVAT != subjecToVAT || skyDiveEvent.StartDate != startDate || skyDiveEvent.EndDate != endDate);
        }
    }
}
