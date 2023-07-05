using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class AdminUserCommand
    {
        public string? Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? NationalCode { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Address { get; set; }

        public float? Height { get; set; }

        public float? Weight { get; set; }

        public string? EmergencyContact { get; set; }

        public string? EmergencyPhone { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Password { get; set; }

        public Guid UserTypeId { get; set; }

        public void Validate()
        {
            new AdminUserCommandValidator().Validate(this).RaiseExceptionIfRequired();
        }
    }
}
