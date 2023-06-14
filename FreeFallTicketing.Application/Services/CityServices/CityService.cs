using SkyDiveTicketing.Application.DTOs;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Services.CityServices
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CityDTO> GetCities(string search)
        {
            return _unitOfWork.CityRepository.GetAll()
                .Where(c => c.City.Contains(search) || c.State.Contains(search) || (c.State + " " + c.City).Contains(search))
                .OrderBy(c => c.State).ThenBy(c=> c.City)
                .Select(city => new CityDTO(city.Id, city.Province, city.State, city.City));
        }
    }
}
