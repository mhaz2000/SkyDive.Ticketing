using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class OtpUserCommand : ICommandBase
    {
        public string Phone { get; set; }

        public void Validate() => new OtpUserCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
