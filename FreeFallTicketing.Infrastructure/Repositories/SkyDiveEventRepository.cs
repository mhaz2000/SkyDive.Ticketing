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

        public void AddFlights(SkyDiveEventItem skyDiveEventDay, IDictionary<SkyDiveEventTicketType, int> typesQty, int flightNumber, int voidableNumber)
        {
            var lastNumber = skyDiveEventDay.FlightLoads.OrderByDescending(s => s.Number).FirstOrDefault()?.Number ?? 0;
            for (int i = 0; i < flightNumber; i++)
            {
                var flightLoad = new FlightLoad(++lastNumber, skyDiveEventDay.Date, skyDiveEventDay.Capacity, voidableNumber);

                foreach (var item in typesQty)
                {
                    flightLoad.FlightLoadItems.Add(new FlightLoadItem(item.Key, item.Value));
                }

                skyDiveEventDay.FlightNumber = flightNumber;
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

        public async Task Create(string title, string location, bool voidable, bool subjecToVAT, Guid image, DateTime startDate, DateTime endDate, SkyDiveEventStatus status)
        {
            var lastCode = Context.SkyDiveEvents.OrderByDescending(x => x.Code).FirstOrDefault()?.Code ?? 0;
            var skyDiveEvent = new SkyDiveEvent(++lastCode, title, location, voidable, subjecToVAT, image, startDate, endDate, status);

            for (int i = 0; i < (endDate - startDate).TotalDays + 1; i++)
            {
                var entity = new SkyDiveEventItem(startDate.AddDays(i), 0);
                await Context.SkyDiveEventItems.AddAsync(entity);

                skyDiveEvent.Items.Add(entity);
            }

            await Context.SkyDiveEvents.AddAsync(skyDiveEvent);
        }

        public IQueryable<SkyDiveEvent> FindEvents(Expression<Func<SkyDiveEvent, bool>>? predicate = null)
        {
            return Context.SkyDiveEvents
                .Include(c => c.TypesAmount).ThenInclude(c => c.Type)
                .Include(c => c.Status)
                .Include(c => c.Items).ThenInclude(c => c.FlightLoads).ThenInclude(c => c.FlightLoadItems).ThenInclude(c => c.Tickets).ThenInclude(c => c.ReservedBy)
                .Where(predicate);
        }

        public IEnumerable<TicketDetailModel> GetDetails(Expression<Func<TicketDetailModel, bool>>? predicate = null)
        {
            return FindEvents()
                   .SelectMany(skyDiveEvent => skyDiveEvent.Items, (skyDiveEvent, skyDiveEventItem) => new { skyDiveEvent, skyDiveEventItem })
                   .SelectMany(c => c.skyDiveEventItem.FlightLoads, (skyDiveEventItem, flightLoad) => new { skyDiveEventItem.skyDiveEvent, skyDiveEventItem.skyDiveEventItem, flightLoad })
                   .SelectMany(c => c.flightLoad.FlightLoadItems, (flightLoad, flightLoadItems) => new { flightLoad.skyDiveEvent, flightLoad.skyDiveEventItem, flightLoad.flightLoad, flightLoadItems })
                   .SelectMany(c => c.flightLoadItems.Tickets, (flightLoadItems, ticket) => new TicketDetailModel
                   {
                       SkyDiveEvent = flightLoadItems.skyDiveEvent,
                       SkyDiveEventItem = flightLoadItems.skyDiveEventItem,
                       FlightLoad = flightLoadItems.flightLoad,
                       FlightLoadItem = flightLoadItems.flightLoadItems,
                       Ticket = ticket
                   }).Where(predicate);
        }

        public void ToggleActivation(SkyDiveEvent skyDiveEvent)
        {
            skyDiveEvent.ToggleActivation();
        }

        public void Update(string title, string location, bool voidable, bool subjecToVAT, Guid image, DateTime startDate, DateTime endDate, SkyDiveEventStatus status, SkyDiveEvent skyDiveEvent)
        {
            skyDiveEvent.Status = status;
            skyDiveEvent.Title = title;
            skyDiveEvent.Location = location;
            skyDiveEvent.Voidable = voidable;
            skyDiveEvent.SubjecToVAT = subjecToVAT;
            skyDiveEvent.Image=image;
            skyDiveEvent.StartDate = startDate;
            skyDiveEvent.EndDate = endDate;
        }
    }
}
