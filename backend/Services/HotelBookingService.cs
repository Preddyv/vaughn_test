using System.Threading.Tasks;
using backend.Models;

namespace backend.Services
{
    public interface IHotelBookingService
    {
        Task<bool> BookHotelAsync(Hotel hotel);
    }

    public class HotelBookingService : IHotelBookingService
    {
        public async Task<bool> BookHotelAsync(Hotel hotel)
        {
            // Implement the hotel booking logic here
            // For now, let's assume the booking is always successful
            hotel.IsBooked = true;
            return await Task.FromResult(true);
        }
    }
}
