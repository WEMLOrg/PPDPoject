namespace TimeTableApp.Models
{
    public class Room
    {
        public Guid _id { get; set; }
        public int capacity { get; set; }

        public Room(Guid id, int capacity)
        {
            this._id = id;
            this.capacity = capacity;
        }

        public static bool operator ==(Room t1, Room t2)
        {
            if (ReferenceEquals(t1, t2)) return true;
            if (t1 is null || t2 is null) return false;
            return t1._id == t2._id;
        }
        public static bool operator !=(Room t1, Room t2)
        {
            return !(t1 == t2);
        }
    }
}