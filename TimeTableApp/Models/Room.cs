namespace TimeTableApp.Models
{
    public class Room 
    {
        private Guid _id { get; set; }
        private int capacity { get; set; }
        public Room(int capacity)
        {
            this.capacity = capacity;
        }
    } }