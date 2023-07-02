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

    public List<BookingDetailsDto>? GetBookingDetails()
    {
        var bookings = _bookingRepository.GetBookingDetails();
        var bookingDetails = bookings.Select(b => new BookingDetailsDto
        {
            Guid = b.Guid,
            BookedNik = b.BookedNik,
            BookedBy = b.BookedBy,
            RoomName = b.RoomName,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            Status = b.Status,
            Remarks = b.Remarks
        }).ToList();

        return bookingDetails;
    }
    public BookingDetailsDto? GetBookingDetailsByGuid(Guid guid)
    {
        var relatedBooking = GetBookingDetails().FirstOrDefault(b => b.Guid == guid);
        return relatedBooking;
    }

    public IEnumerable<BookingLengthDto>? BookingDuration()
    {
        var bookings = _bookingRepository.GetAll();
        var rooms = _roomRepository.GetAll();

        var entities = (from b in bookings
                        join r in rooms on b.RoomGuid equals r.Guid
                        select new
                        {
                            guid = r.Guid,
                            startDate = b.StartDate,
                            endDate = b.EndDate,
                            roomName = r.Name
                        }).ToList();

        var bookingDurations = new List<BookingLengthDto>();

        foreach (var entity in entities)
        {
            TimeSpan duration = entity.endDate - entity.startDate;

            int totalDays = (int)duration.Days;
            int weekends = 0;

            for (int i = 0; i <= totalDays; i++)
            {
                var currentDate = entity.startDate.AddDays(i);
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekends++;
                }
            }

            TimeSpan bookingLength = duration - TimeSpan.FromDays(weekends);
            var bookingDurationDto = new BookingLengthDto
            {
                RoomGuid = entity.guid,
                RoomName = entity.roomName,
                BookingLength = bookingLength
            };
            bookingDurations.Add(bookingDurationDto);
        }
        return bookingDurations;
    }
}
