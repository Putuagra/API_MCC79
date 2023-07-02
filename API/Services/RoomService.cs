using API.Contracts;
using API.DTOs.Rooms;
using API.Models;

namespace API.Services;

public class RoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;
    public RoomService(IRoomRepository roomRepository, IBookingRepository bookingRepository)
    {
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
    }

    public IEnumerable<GetRoomDto>? GetRoom()
    {
        var rooms = _roomRepository.GetAll();
        if (!rooms.Any())
        {
            return null;
        }

        var toDto = rooms.Select(room => new GetRoomDto
        {
            Guid = room.Guid,
            Name = room.Name,
            Floor = room.Floor,
            Capacity = room.Capacity
        }).ToList();

        return toDto;
    }

    public GetRoomDto? GetRoom(Guid guid)
    {
        var room = _roomRepository.GetByGuid(guid);

        if (room is null)
        {
            return null;
        }

        var toDto = new GetRoomDto
        {
            Guid = room.Guid,
            Name = room.Name,
            Floor = room.Floor,
            Capacity = room.Capacity
        };

        return toDto;
    }

    public GetRoomDto? CreateRoom(NewRoomDto newRoomDto)
    {
        var room = new Room
        {
            Guid = new Guid(),
            Name = newRoomDto.Name,
            Floor = newRoomDto.Floor,
            Capacity = newRoomDto.Capacity,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdRoom = _roomRepository.Create(room);

        if (createdRoom == null)
        {
            return null;
        }

        var toDto = new GetRoomDto
        {
            Guid = createdRoom.Guid,
            Name = createdRoom.Name,
            Floor = createdRoom.Floor,
            Capacity = createdRoom.Capacity
        };

        return toDto;
    }

    public int UpdateRoom(GetRoomDto getRoomDto)
    {
        var isExist = _roomRepository.IsExist(getRoomDto.Guid);

        if (!isExist)
        {
            return -1;
        }

        var getUniversity = _roomRepository.GetByGuid(getRoomDto.Guid);
        var room = new Room
        {
            Guid = getRoomDto.Guid,
            Name = getRoomDto.Name,
            Floor = getRoomDto.Floor,
            Capacity = getRoomDto.Capacity,
            ModifiedDate = DateTime.Now,
            CreatedDate = getUniversity.CreatedDate
        };

        var isUpdate = _roomRepository.Update(room);

        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteRoom(Guid guid)
    {
        var isExist = _roomRepository.IsExist(guid);

        if (!isExist)
        {
            return -1;
        }

        var room = _roomRepository.GetByGuid(guid);
        var isDelete = _roomRepository.Delete(room!);
        if (!isDelete)
        {
            return 0;
        }
        return 1;
    }

    public IEnumerable<UnusedRoomDto> GetUnusedRooms()
    {
        var rooms = _roomRepository.GetAll().ToList();

        var usedRooms = from r in _roomRepository.GetAll()
                        join b in _bookingRepository.GetAll() on r.Guid equals b.RoomGuid
                        where b.Status == Utilites.Enums.StatusLevel.OnGoing
                        select new UnusedRoomDto
                        {
                            RoomGuid = r.Guid,
                            RoomName = r.Name,
                            Floor = r.Floor,
                            Capacity = r.Capacity,
                        };

        List<Room> tmpRooms = new List<Room>(rooms);
        foreach (var room in rooms)
        {
            foreach (var usedRoom in usedRooms)
            {
                if (room.Guid == usedRoom.RoomGuid)
                {
                    tmpRooms.Remove(room);
                    break;
                }
            }
        }
        var unusedRooms = from room in tmpRooms
                          select new UnusedRoomDto
                          {
                              RoomGuid = room.Guid,
                              RoomName = room.Name,
                              Floor = room.Floor,
                              Capacity = room.Capacity
                          };
        return unusedRooms;
    }
}
