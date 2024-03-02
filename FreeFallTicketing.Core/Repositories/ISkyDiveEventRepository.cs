using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Model;
using SkyDiveTicketing.Core.Repositories.Base;
using System.Linq.Expressions;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface ISkyDiveEventRepository : IRepository<SkyDiveEvent>
    {
        IQueryable<SkyDiveEvent> FindEvents(Expression<Func<SkyDiveEvent, bool>>? predicate = null);
        Task Create(string title, string location, bool voidable, bool subjecToVAT, Guid? image, DateTime startDate, DateTime endDate, SkyDiveEventStatus status);
        void ToggleActivation(SkyDiveEvent skyDiveEvent);
        Task Update(string title, string location, bool voidable, bool subjecToVAT, Guid? image,
            DateTime startDate, DateTime endDate, SkyDiveEventStatus status, SkyDiveEvent skyDiveEvent);
        Task AddFlightsAsync(SkyDiveEvent skyDiveEvent, SkyDiveEventItem skyDiveEventDay, IDictionary<SkyDiveEventTicketType, int> typesQty, int flightNumber, int voidableNumber, int capacity);
        Task AddTicketTypeAmount(SkyDiveEvent skyDiveEvent, SkyDiveEventTicketType ticketType, double amount);
        void AddConditionsAndTerms(SkyDiveEvent skyDiveEvent, string conditionsAndTerms);
        void ClearTicketTypesAmount(SkyDiveEvent skyDiveEvent);
        Task<IEnumerable<SkyDiveEventTicketTypeAmount>?> GetSkyDiveEventTicketTypesAmount(Guid id);
        Task<IEnumerable<TicketDetailModel>> GetExpandedSkyDiveEvent(Guid id);
        void PublishEvent(SkyDiveEvent skyDiveEvent);
        Task<bool> HasFlightLoad(Guid id);
        Task<bool> CheckIfCriticalDataIsChanged(SkyDiveEvent skyDiveEvent, string title, string location, bool voidable,
            bool subjecToVAT, DateTime startDate, DateTime endDate);
        Task<IEnumerable<FlightLoad>?> GetSkyDiveEventDayFlights(Guid id);
    }
}
