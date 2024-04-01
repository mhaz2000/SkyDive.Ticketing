using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.ReportCommands;
using SkyDiveTicketing.Application.DTOs.ReportDTOs;
using SkyDiveTicketing.Application.DTOs.TicketDTOs;
using SkyDiveTicketing.Application.Helpers;
using SkyDiveTicketing.Core.Repositories.Base;
using System.Globalization;

namespace SkyDiveTicketing.Application.Services.ReportServices
{
    internal class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;

        public ReportService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }

        public async Task<(IQueryable<TicketReportDTO> Data, int Total, Guid cacheId)> GetTicketsReport(TicketReportCommand command)
        {
            Guid cacheId = Guid.NewGuid();

            var skyDiveEvents = _unitOfWork.SkyDiveEventRepository.FindEvents(c => command.EventsId.Any() ? command.EventsId.Contains(c.Id) : true
                && c.Title.Contains(command.Search)).ToList();

            var canceledTickets = _unitOfWork.TicketRepository.GetCancelledTickets().ToList();

            var result = skyDiveEvents
                .SelectMany(skyDiveEvent => skyDiveEvent.Items, (skyDiveEvent, eventItem) => new
                {
                    skyDiveEvent,
                    eventItem
                })
                .SelectMany(s => s.eventItem.FlightLoads, (skyDiveEvent_eventItem, flightLoad) => new
                {
                    skyDiveEvent_eventItem.skyDiveEvent,
                    skyDiveEvent_eventItem.eventItem,
                    flightLoad
                })
                .SelectMany(s => s.flightLoad.FlightLoadItems, (skyDiveEvent_eventItem_flightLoad, flighLoadItem) => new
                {
                    skyDiveEvent_eventItem_flightLoad.skyDiveEvent,
                    skyDiveEvent_eventItem_flightLoad.eventItem,
                    skyDiveEvent_eventItem_flightLoad.flightLoad,
                    flighLoadItem
                })
                .SelectMany(s => s.flighLoadItem.Tickets, (skyDiveEvent_eventItem_flightLoad_flightLoadItem, ticket) => new
                {
                    skyDiveEvent_eventItem_flightLoad_flightLoadItem.skyDiveEvent,
                    skyDiveEvent_eventItem_flightLoad_flightLoadItem.eventItem,
                    skyDiveEvent_eventItem_flightLoad_flightLoadItem.flightLoad,
                    skyDiveEvent_eventItem_flightLoad_flightLoadItem.flighLoadItem,
                    ticket
                }).Where(c => c.ticket.ReservedForId is not null).Select(data => new TicketReportDTO()
                {
                    EventCode = data.skyDiveEvent.Code,
                    EventTitle = data.skyDiveEvent.Title,
                    EventDate = data.skyDiveEvent.StartDate.ToString("yyyy/MM/dd", CultureInfo.GetCultureInfo("fa-ir")),
                    FlightDate = data.flightLoad.Date.ToString("yyyy/MM/dd", CultureInfo.GetCultureInfo("fa-ir")),
                    FlightName = data.flightLoad.Name,
                    FlightNumber = data.flightLoad.Number,
                    FlightStatus = data.flightLoad.Status.GetDescription(),
                    FullName = data.ticket.ReservedFor.FullName,
                    Height = data.ticket.ReservedFor.Passenger.Height,
                    Weight = data.ticket.ReservedFor.Passenger.Weight,
                    PhoneNumber = data.ticket.ReservedFor.PhoneNumber,
                    NationalCode = data.ticket.ReservedFor.NationalCode,
                    TicketNumber = data.ticket.TicketNumber,
                    TicketType = data.flighLoadItem.FlightLoadType.Title,
                    UserCode = data.ticket.ReservedFor.Code,
                    TicketStatus = canceledTickets.Any(c => c.TicketNumber == data.ticket.TicketNumber) ? TicketStatus.Cancelled.GetDescription() :
                        data.skyDiveEvent.StartDate >= DateTime.Now ? TicketStatus.Reserved.GetDescription() : TicketStatus.Held.GetDescription()
                });

            result = result.Where(c => (c.EventTitle + c.FlightNumber + c.EventDate + c.FullName + c.TicketNumber + c.NationalCode + c.UserCode + c.TicketType).Contains(command.Search));

            _memoryCache.Set(cacheId, JsonConvert.SerializeObject(result), TimeSpan.FromHours(1));

            return await Task.FromResult((result.AsQueryable(), result.Count(), cacheId));
        }


        public Task<MemoryStream> PrintTicketsReport(Guid id)
        {
            var cachedData = _memoryCache.Get(id)?.ToString();
            if (cachedData is null)
                throw new ManagedException("لطفا مجدد گزارش گیری فرمایید.");

            var data = JsonConvert.DeserializeObject<IEnumerable<TicketReportDTO>>(cachedData);

            return Task.FromResult(ExcelHelper.GenerateExcel(data, GetTicketReportHeaders()));
        }

        private Dictionary<string, string> GetTicketReportHeaders()
        {
            return new Dictionary<string, string>()
            {
                {"eventcode","کد رویداد"},
                {"eventtitle","نام رویداد"},
                {"eventdate","تاریخ شروع رویداد"},
                {"flightname","نام پرواز"},
                {"flightstatus","وضعیت پرواز"},
                {"flightdate","تاریخ پرواز"},
                {"flightnumber","شماره پرواز"},
                {"ticketnumber","شماره بلیت"},
                {"tickettype","نوع بلیت"},
                {"ticketstatus","وضعیت بلیت"},
                {"fullname","نام و نام خانوادگی"},
                {"nationalcode","ککد ملی"},
                {"phonenumber","موبایل"},
                {"weight","وزن"},
                {"height","قد"}
            };
        }
    }
}
