﻿using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using SkyDiveTicketing.Application.Services.SkyDiveEventServices;
using System.Data;

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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SkyDiveEventCommand command)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("ConditionsAndTerms/{id}")]
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpGet]
        public IActionResult Get(Guid? statusId, DateTime? start, DateTime? end, PageQuery page)
        {
            try
            {
                var events = _skyDiveEventService.GetEvents(statusId, start, end);
                return OkResult("اطلاعات رویداد ها.", events.ToPagingAndSorting(page), events.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpGet("EventDays/{id}")]
        public async Task<IActionResult> GetEventDays(Guid id)
        {
            try
            {
                var eventDays = await _skyDiveEventService.GetEventDays(id, UserId);
                return OkResult("روز های رویداد", eventDays, eventDays.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpGet("EventDayFlights/{id}")]
        public IActionResult GetEventDayFlights(Guid id, PageQuery page)
        {
            try
            {
                var tickets = _skyDiveEventService.GetEventDayFlights(id, page.PageSize);
                return OkResult("بلیت های رویداد", tickets, tickets.Flights.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }
    }
}