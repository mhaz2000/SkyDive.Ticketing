﻿using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;
using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class UpdateUserCommand : UserCommand
    {
        public string? Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? NationalCode { get; set; }

        public Guid? CityId { get; set; }

        public string? Address { get; set; }

        public float? Height { get; set; }

        public float? Weight { get; set; }

        public string? EmergencyContact { get; set; }

        public string? EmergencyPhone { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }


        public override void Validate()
        {
            base.Validate();
            new UpdateUserCommandValidator().Validate(this).RaiseExceptionIfRequired();
        }
    }
}
