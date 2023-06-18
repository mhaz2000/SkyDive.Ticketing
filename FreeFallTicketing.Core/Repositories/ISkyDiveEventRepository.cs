using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Model;
using SkyDiveTicketing.Core.Repositories.Base;
using System.Linq.Expressions;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface ISkyDiveEventRepository : IRepository<SkyDiveEvent>
    {
        IQueryable<SkyDiveEvent> FindEvents(Expression<Func<SkyDiveEvent, bool>>? predicate = null);
        Task Create(string title, string location, bool voidable, bool subjecToVAT, Guid image, DateTime startDate, DateTime endDate, SkyDiveEventStatus status);
        void ToggleActivation(SkyDiveEvent skyDiveEvent);
        void Update(string title, string location, bool voidable, bool subjecToVAT, Guid image, DateTime startDate, DateTime endDate, SkyDiveEventStatus status, SkyDiveEvent skyDiveEvent);
        void AddFlights(SkyDiveEventItem skyDiveEventDay, IDictionary<SkyDiveEventTicketType, int> typesQty, int flightNumber, int voidableNumber);
        Task AddTicketTypeAmount(SkyDiveEvent skyDiveEvent, SkyDiveEventTicketType ticketType, double amount);
        void AddConditionsAndTerms(SkyDiveEvent skyDiveEvent, string conditionsAndTerms);
        void ClearTicketTypesAmount(SkyDiveEvent skyDiveEvent);
        IEnumerable<TicketDetailModel> GetDetails(Expression<Func<TicketDetailModel, bool>>? predicate = null);
    }
}
