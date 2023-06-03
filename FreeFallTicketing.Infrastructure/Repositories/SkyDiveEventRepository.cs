using Microsoft.EntityFrameworkCore;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;
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

        public void AddTicketTypeAmount(SkyDiveEvent skyDiveEvent, SkyDiveEventTicketType ticketType, double amount)
        {
            skyDiveEvent.TypesAmount.Add(new SkyDiveEventTicketTypeAmount(ticketType, amount));
        }

        public void ClearTicketTypesAmount(SkyDiveEvent skyDiveEvent)
        {
            skyDiveEvent.TypesAmount.Clear();
        }

        public async Task Create(int code, string title, string location, bool voidable, bool subjecToVAT, Guid image, DateTime startDate, DateTime endDate, SkyDiveEventStatus status)
        {
            var skyDiveEvent = new SkyDiveEvent(code, title, location, voidable, subjecToVAT, image, startDate, endDate, status);

            await Context.SkyDiveEvents.AddAsync(skyDiveEvent);
        }

        public IQueryable<SkyDiveEvent> FindEvents(Expression<Func<SkyDiveEvent, bool>>? predicate = null)
        {
            return Context.SkyDiveEvents
                .Include(c => c.TypesAmount).ThenInclude(c => c.Type)
                .Include(c => c.Status)
                .Include(c => c.Items).ThenInclude(c => c.FlightLoads).ThenInclude(c=>c.FlightLoadItems).ThenInclude(c=>c.Tickets).ThenInclude(c=> c.ReservedBy)
                .Where(predicate);
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

            skyDiveEvent.Images.Clear(); //temporary
            skyDiveEvent.Images.Add(image);

            skyDiveEvent.StartDate = startDate;
            skyDiveEvent.EndDate = endDate;

        }
    }
}
