using TimeTableApp.Models;
using System.Xml;
using System.Xml.Linq;

namespace TimeTableApp.Repository;

public class RoomsRepository
{
    private List<Room> RoomsList = new List<Room>();

    public RoomsRepository()
    {
        string filePath = generateDefaultFilePath();
        if (!File.Exists(filePath))
        {
            Console.WriteLine("pula shein");
        }
        loadData(filePath);

    }
    private string generateDefaultFilePath()
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Rooms.xml");
    }

    private void loadData(string filePath)
    {
        Console.WriteLine("Reading rooms data from file: " + filePath);

        XDocument xDocument = XDocument.Load(filePath);
        XElement root = xDocument.Element("Rooms");
        if (root != null && root.HasElements)
        {
            foreach (var elem in root.Elements("Room"))
            {
                Guid Id;
                if (!Guid.TryParse((string)elem.Attribute("Id"), out Id))
                {
                    continue;
                }
                int capacity;
                if (!int.TryParse((string)elem.Attribute("Cap"), out capacity))
                {
                    continue;
                }

                Room room = new Room(Id, capacity);
                RoomsList.Add(room);

            }
        }
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