﻿using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.API.Extensions;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using SkyDiveTicketing.Application.Services.SkyDiveEventServices;

namespace SkyDiveTicketing.API.Controllers.SkyDiveEvents
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkyDiveEventsController : ApiControllerBase
    {
        private readonly ISkyDiveEventService _skyDiveEventService;

        public SkyDiveEventsController(ISkyDiveEventService skyDiveEventService)
        {
            _skyDiveEventService = skyDiveEventService;
        }

        #region Admin Apies

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(SkyDiveEventCommand command)
        {
            try
            {
                command.Validate();

                await _skyDiveEventService.Create(command);
                return OkResult("رویداد جدید با موفقیت ثبت شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SkyDiveEventUpdateCommand command)
        {
            try
            {
                command.Validate();

                await _skyDiveEventService.Update(command, id);
                return OkResult("رویداد با موفقیت ویرایش شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("ToggleActivation/{id}")]
        public async Task<IActionResult> ToggleActivation(Guid id)
        {
            try
            {
                await _skyDiveEventService.ToggleActivationEvent(id);
                return OkResult("عملیات با موفقیت انجام شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _skyDiveEventService.Remove(id);
                return OkResult("رویداد با موفقیت حذف شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddEventTypeFee/{id}")]
        public async Task<IActionResult> AddEventTypeFee(Guid id, [FromBody] AddEventTypeFeeCommand command)
        {
            try
            {
                command.Validate();

                await _skyDiveEventService.AddEventTypeFee(command, id);
                return OkResult("قیمت های واحد برای انواع بلیت ثبت شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("TicketTypeAmounts/{id}")]
        public async Task<IActionResult> GetTicketTypeAmounts(Guid id)
        {
            try
            {
                var typesAmount = await _skyDiveEventService.GetEventTicketTypeAmounts(id);
                return OkResult("نرخ انواع بلیت ها.", typesAmount, typesAmount.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("RemoveFlights/{id}")]
        public async Task<IActionResult> RemoveFlights(Guid id)
        {
            try
            {
                await _skyDiveEventService.RemoveFlights(id);
                return OkResult("پرواز ها با موفقیت حذف شدند.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("RemoveFlight/{id}")]
        public async Task<IActionResult> RemoveFlight(Guid id)
        {
            try
            {
                await _skyDiveEventService.RemoveFlight(id);
                return OkResult("پرواز مورد نظر با موفقیت حذف شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("ConditionsAndTerms/{id}")]
        public async Task<IActionResult> AddConditionsAndTerms(Guid id, [FromBody] AddEventConditionsAndTermsCommand command)
        {
            try
            {
                command.Validate();

                await _skyDiveEventService.AddConditionsAndTerms(command, id);
                return OkResult("شرایط و قوانین با موفقیت ثبت شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddFlight/{id}")]
        public async Task<IActionResult> AddFlight(Guid id, [FromBody] AddSkyDiveEventFlightCommand command)
        {
            try
            {
                command.Validate();

                await _skyDiveEventService.AddFlight(command, id);
                return OkResult("پرواز های جدید با موفقیت ثبت گردید.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("SetFlightStatus/{id}")]
        public async Task<IActionResult> SetFlightStatus(Guid id, [FromBody] SetFlightStatusCommand command)
        {
            try
            {
                command.Validate();

                await _skyDiveEventService.SetFlightStatus(command, id);
                return OkResult("پرواز با موفقیت ویرایش شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("SetFlightName/{id}")]
        public async Task<IActionResult> SetFlightName(Guid id, [FromBody] SetFlightNameCommand command)
        {
            try
            {
                command.Validate();

                await _skyDiveEventService.SetFlightName(command, id);
                return OkResult("پرواز با موفقیت ویرایش شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("EventDayFlights/{id}")]
        public IActionResult GetEventDayFlights(Guid id, [FromQuery] PageQuery page)
        {
            try
            {
                var tickets = _skyDiveEventService.GetEventDayFlights(id, page.PageSize, page.PageIndex);
                return OkResult("بلیت های رویداد", tickets, tickets.Flights.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Tickets/{flightId}")]
        public async Task<IActionResult> GetTickets(Guid flightId, [FromQuery] PageQuery page)
        {
            try
            {
                var tickets = await _skyDiveEventService.GetFlightTickets(flightId);
                return OkResult("بلیت های رویداد", tickets.ToPagingAndSorting(page), tickets.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateTicket")]
        public async Task<IActionResult> UpdateTicket(UpdateTicketCommand command)
        {
            try
            {
                await _skyDiveEventService.UpdateTicket(command);
                return OkResult("ویرایش بلیت با موفقیت انجام شد.");
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("RemoveTicket/{id}")]
        public async Task<IActionResult> RemoveTicket(Guid id)
        {
            try
            {
                await _skyDiveEventService.RemoveTicket(id);
                return OkResult("بلیت مورد نظر حذف گردید..");
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("PublishEvent/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PublishEvent(Guid id)
        {
            try
            {
                await _skyDiveEventService.PublishEvent(id);
                return OkResult("رویداد با موفقیت فعال شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion

        [HttpGet]
        public IActionResult Get([FromQuery] PageQuery page, Guid? statusId, string? start, string? end)
        {
            try
            {
                var min = start?.ToDateTime();
                var max = end?.ToDateTime();

                var events = _skyDiveEventService.GetEvents(statusId, min, max, UserId);
                return OkResult("اطلاعات رویداد ها.", events.ToPagingAndSorting(page), events.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var skyDiveEvent = await _skyDiveEventService.GetEvent(id);
                return OkResult("اطلاعات رویداد.", skyDiveEvent);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetLastCode")]
        public IActionResult GetLastCode(Guid id)
        {
            try
            {
                var lastCode = _skyDiveEventService.GetLastCode(id);
                return OkResult("آخرین کد رویداد", lastCode);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("EventDays/{id}")]
        public async Task<IActionResult> GetEventDays(Guid id)
        {
            try
            {
                (var eventName, var eventDays) = await _skyDiveEventService.GetEventDays(id, UserId);
                return OkResult(eventName, eventDays, eventDays.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("EventDayTickets/{id}")]
        public async Task<IActionResult> GetEventDayTickets(Guid id, [FromQuery] PageQuery pageQuery)
        {
            try
            {
                (var tickets, var count) = await _skyDiveEventService.GetEventDayTickets(id, pageQuery.PageIndex, pageQuery.PageSize, UserId);
                return OkResult("بلیت های روز", tickets, count);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}