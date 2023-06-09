﻿using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class UserResetPasswordCommand : ICommandBase
    {
        public string? Password { get; set; }

        public void Validate() => new UserResetPasswordCommandValidator().Validate(this).RaiseExceptionIfRequired();

    }
}
