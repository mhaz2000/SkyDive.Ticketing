﻿using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;
using System.Text.RegularExpressions;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class AdminUserCommandValidator : AbstractValidator<AdminUserCommand>
    {
        public AdminUserCommandValidator()
        {
            RuleFor(c=> c.Username).NotEmpty().WithMessage("نام کاربری نمی‌تواند خالی باشد.");
            RuleFor(c=> c.Phone).NotEmpty().WithMessage("شماره موبایل نمی‌تواند خالی باشد.");
            RuleFor(c=> c.Height).GreaterThan(0).When(c=> c.Height is not null).WithMessage("قد نمی‌تواند منفی باشد.");
            RuleFor(c=> c.Weight).GreaterThan(0).When(c=> c.Weight is not null).WithMessage("وزن نمی‌تواند منفی باشد.");
            RuleFor(c => c.BirthDate).LessThan(DateTime.Now).When(c => c.BirthDate is not null).WithMessage("تاریخ تولد نمی‌تواند بزرگتر از تاریخ امروز باشد.");

            var passwordReg = new Regex(@"^(?=.*[0-9])(?=.*[A-Za-z]).{6,}$"); 
            var usernameReg = new Regex(@"^(?=.{5,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$");

            RuleFor(c => c.Password).Must(pass => passwordReg.IsMatch(pass)).When(c=> string.IsNullOrEmpty(c.Password))
                .WithMessage("رمز باید حداقل 6 کاراکتر و شامل اعداد و حروف باشد!");

            RuleFor(c => c.Username).Must(username => usernameReg.IsMatch(username))
                .When(c=> string.IsNullOrEmpty(c.Username)).WithMessage("نام کاربری اشتباه است!");
        }
    }
}
