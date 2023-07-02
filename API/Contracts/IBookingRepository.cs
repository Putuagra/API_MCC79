using API.DTOs.Bookings;
using API.Models;

namespace API.Contracts;

public interface IBookingRepository : IGeneralRepository<Booking>
{
    ICollection<GetRoomTodayDto> GetByDateNow();
    IEnumerable<BookingDetailsDto> GetBookingDetails();
}
