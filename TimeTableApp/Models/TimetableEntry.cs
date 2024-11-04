namespace TimeTableApp.Models;

public class TimetableEntry
{
    public Teacher teacher { get; set; }
    public Subject subject { get; set; }
    public Group group { get; set; }
    public Room room { get; set; }

    TimetableEntry(Teacher t, Subject s, Group g, Room r) 
    {
        teacher = t;
        subject = s;
        group = g;
        room = r;
    }
}