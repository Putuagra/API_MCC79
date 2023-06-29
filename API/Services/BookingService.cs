using API.Contracts;
using API.DTOs.Bookings;
using API.Models;

namespace API.Services;

public class BookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository, IEmployeeRepository employeeRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _employeeRepository = employeeRepository;
    }

    public IEnumerable<GetBookingDto>? GetBooking()
    {
        var bookings = _bookingRepository.GetAll();
        if (!bookings.Any())
        {
            return null;
        }

        var toDto = bookings.Select(booking => new GetBookingDto
        {
            Guid = booking.Guid,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Status = booking.Status,
            Remarks = booking.Remarks,
            RoomGuid = booking.RoomGuid,
            EmployeeGuid = booking.EmployeeGuid
        }).ToList();


        return toDto;
    }

    public GetBookingDto? GetBooking(Guid guid)
    {
        var booking = _bookingRepository.GetByGuid(guid);
        if (booking is null)
        {
            return null;
        }

        var toDto = new GetBookingDto
        {
            Guid = booking.Guid,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Status = booking.Status,
            Remarks = booking.Remarks,
            RoomGuid = booking.RoomGuid,
            EmployeeGuid = booking.EmployeeGuid
        };

        return toDto;
    }

    public GetBookingDto? CreateBooking(NewBookingDto newBookingDto)
    {
        var booking = new Booking
        {
            Guid = new Guid(),
            StartDate = newBookingDto.StartDate,
            EndDate = newBookingDto.EndDate,
            Status = newBookingDto.Status,
            Remarks = newBookingDto.Remarks,
            RoomGuid = newBookingDto.RoomGuid,
            EmployeeGuid = newBookingDto.EmployeeGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdBooking = _bookingRepository.Create(booking);
        if (createdBooking is null)
        {
            return null;
        }

        var toDto = new GetBookingDto
        {
            Guid = createdBooking.Guid,
            StartDate = newBookingDto.StartDate,
            EndDate = newBookingDto.EndDate,
            Status = newBookingDto.Status,
            Remarks = newBookingDto.Remarks,
            RoomGuid = newBookingDto.RoomGuid,
            EmployeeGuid = newBookingDto.EmployeeGuid,
        };

        return toDto;
    }

    public int UpdateBooking(GetBookingDto getBookingDto)
    {
        var isExist = _bookingRepository.IsExist(getBookingDto.Guid);
        if (!isExist)
        {
            return -1; // Booking not found
        }

        var getBooking = _bookingRepository.GetByGuid(getBookingDto.Guid);

        var booking = new Booking
        {
            Guid = getBookingDto.Guid,
            StartDate = getBookingDto.StartDate,
            EndDate = getBookingDto.EndDate,
            Status = getBookingDto.Status,
            Remarks = getBookingDto.Remarks,
            RoomGuid = getBookingDto.RoomGuid,
            EmployeeGuid = getBookingDto.EmployeeGuid,
            ModifiedDate = DateTime.Now,
            CreatedDate = getBooking!.CreatedDate
        };

        var isUpdate = _bookingRepository.Update(booking);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteBooking(Guid guid)
    {
        var isExist = _bookingRepository.IsExist(guid);
        if (!isExist)
        {
            return -1;
        }

        var booking = _bookingRepository.GetByGuid(guid);
        var isDelete = _bookingRepository.Delete(booking!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }

    public IEnumerable<GetRoomTodayDto> GetRoomToday()
    {
        var room = (from r in _roomRepository.GetAll()
                    join b in _bookingRepository.GetAll() on r.Guid equals b.RoomGuid
                    join e in _employeeRepository.GetAll() on b.EmployeeGuid equals e.Guid
                    where b.StartDate <= DateTime.Now && b.EndDate >= DateTime.Now
                    select new GetRoomTodayDto
                    {
                        Guid = b.Guid,
                        RoomName = r.Name,
                        Status = b.Status,
                        Floor = r.Floor,
                        BookedBy = e.FirstName + e.LastName
                    }).ToList();

        if (!room.Any())
        {
            return null;
        }
        var toDto = room.Select(r => new GetRoomTodayDto
        {
            Guid = r.Guid,
            RoomName = r.RoomName,
            Status = r.Status,
            Floor = r.Floor,
            BookedBy = r.BookedBy
        });
        return toDto;
    }
}
