using TimeTableApp.Models;

namespace TimeTableApp.Repository;

public class RoomsRepository
{
    private List<Room> RoomsList;

    RoomsRepository()
    {
        RoomsList.Add(new Room(30));
        RoomsList.Add(new Room(50));
        RoomsList.Add(new Room(35));
        RoomsList.Add(new Room(45));
        RoomsList.Add(new Room(100));
        RoomsList.Add(new Room(56));
        RoomsList.Add(new Room(80));
        RoomsList.Add(new Room(20));
        RoomsList.Add(new Room(80));
        RoomsList.Add(new Room(100));
    }
    
    public void AddRoom(Room room)
    {
        RoomsList.Add(room);
    }
    public List<Room> GetRooms()
    {
        return RoomsList;
    }
}