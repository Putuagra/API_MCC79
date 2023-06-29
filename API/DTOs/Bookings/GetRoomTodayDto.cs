using API.Utilites.Enums;

namespace API.DTOs.Bookings;

public class GetRoomTodayDto
{
    public Guid Guid { get; set; }
    public string RoomName { get; set; }
    public StatusLevel Status { get; set; }
    public int Floor { get; set; }
    public string BookedBy { get; set; }

}
