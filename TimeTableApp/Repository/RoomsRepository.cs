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
            CreateNewXmlFile(filePath);
            CreateDefaultRoomsFile(filePath);
            loadData(filePath);

        }
        loadData(filePath);
    }
    private void CreateNewXmlFile(string filePath)
    {
        XDocument newDocument = new XDocument(
            new XElement("Rooms")
        );

        newDocument.Save(filePath);
        Console.WriteLine("New Rooms.xml file created at: " + filePath);
    }
    private string generateDefaultFilePath()
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Rooms.xml");
    }

    private void loadData(string filePath)
    {
        Console.WriteLine("Reading rooms data from file: " + filePath);

        try
        {
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
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while loading rooms data: " + ex.Message);
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

    private void CreateDefaultRoomsFile(string filePath)
    {
        Console.WriteLine("Creating default rooms data file: " + filePath);

        XDocument xDocument = new XDocument(
            new XElement("Rooms",
                new XElement("Room",
                    new XAttribute("Id", "a16e8d98-5c24-4a37-824f-2dcfb5f246a3"),
                    new XAttribute("Cap", 30)
                ),
                new XElement("Room",
                    new XAttribute("Id", "b59f16b7-e30c-4cde-8a3c-1f239fb367de"),
                    new XAttribute("Cap", 50)
                ),
                new XElement("Room",
                    new XAttribute("Id", "c4038de8-a58e-46bc-8247-6a3e9ecf8471"),
                    new XAttribute("Cap", 100)
                ),
                new XElement("Room",
                    new XAttribute("Id", "g9038de8-a58e-46bc-8247-6a3e9ecf8471"),
                    new XAttribute("Cap", 100)
                ),
                new XElement("Room",
                    new XAttribute("Id", "m4038de8-a58e-46bc-8247-6m3e9ecf8471"),
                    new XAttribute("Cap", 100)
                ),
                new XElement("Room",
                    new XAttribute("Id", "k4038de8-a58e-46bk-8247-6a3e9ecf8471"),
                    new XAttribute("Cap", 100)
                )
            )
        );

        try
        {
            xDocument.Save(filePath);
            Console.WriteLine("Rooms file created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while creating the default rooms file: " + ex.Message);
        }
    }

 }
