using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using backend.Models;

namespace backend.Services
{
    public interface IHotelBookingService
    {
        Task<bool> BookHotelAsync(Hotel hotel);
        Task<IEnumerable<Hotel>> GetHotelsAsync();
    }

    public class HotelBookingService : IHotelBookingService
    {
        private static readonly List<Hotel> _hotels = new List<Hotel>
        {
            new Hotel { Name = "Grand Hotel", Latitude = 40.7128, Longitude = -74.0060, IsBooked = false },
            new Hotel { Name = "Seaside Resort", Latitude = -43.9509, Longitude = -34.4618, IsBooked = false },
            new Hotel { Name = "Mountain Lodge", Latitude = 34.0522, Longitude = -118.2437, IsBooked = false },
            new Hotel { Name = "Desert Oasis", Latitude = -25.2744, Longitude = 133.7751, IsBooked = false }
        };

        public async Task<bool> BookHotelAsync(Hotel hotel)
        {
            var existingHotel = _hotels.FirstOrDefault(h => h.Name == hotel.Name);
            if (existingHotel != null)
            {
                if (existingHotel.IsBooked)
                {
                    return await Task.FromResult(false);
                }
                existingHotel.IsBooked = true;
                return await Task.FromResult(true);
            }

            hotel.IsBooked = true;
            _hotels.Add(hotel);
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<Hotel>> GetHotelsAsync()
        {
            return await Task.FromResult(_hotels);
        }
    }
}
