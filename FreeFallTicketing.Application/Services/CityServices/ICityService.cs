using SkyDiveTicketing.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Application.Services.CityServices
{
    public interface ICityService
    {
        IEnumerable<CityDTO> GetCities(string search);
    }
}
