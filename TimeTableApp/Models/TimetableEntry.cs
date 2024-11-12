namespace TimeTableApp.Models;

public class TimetableEntry
{
    public enum Days
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday
    }

    public Teacher teacher { get; set; }
    public Subject subject { get; set; }
    public Group group { get; set; }
    public Room room { get; set; }

    public Days day { get; set; }
    public int hour { get; set; }

    public TimetableEntry(Teacher t, Subject s, Group g, Room r, Days d, int h)
    {
        teacher = t;
        subject = s;
        group = g;
        room = r;
        day = d;
        hour = h;
    }

    public static bool operator ==(TimetableEntry t1, TimetableEntry t2)
    {
        if (ReferenceEquals(t1, t2)) return true;
        if (t1 is null || t2 is null) return false;

        return t1.hour == t2.hour &&
               t1.day == t2.day &&
               t1.teacher == t2.teacher &&
               t1.group == t2.group &&
               t1.room == t2.room;
    }
    public static bool operator !=(TimetableEntry t1, TimetableEntry t2)
    {
        return !(t1 == t2);
    }
}