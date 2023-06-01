using SkyDiveTicketing.Application.Services.PassengerServices;

namespace SkyDiveTicketing.API.Jobs
{
    public class PassengerDocumentJob : IPassengerDocumentJob
    {
        private readonly IPassengerService _passengerService;

        public PassengerDocumentJob(IPassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        public async Task CheckPassengerDocumentExpirationDate()
        {
            await _passengerService.CheckPassengerDocumentExpirationDate();
        }
    }
}
